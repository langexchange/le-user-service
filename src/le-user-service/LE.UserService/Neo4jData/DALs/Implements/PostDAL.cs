using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs.NodeRelationConstants;
using LE.UserService.Neo4jData.DALs.Schemas;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs.Implements
{
    public class PostDAL : BaseDAL, IPostDAL
    {
        private readonly ILogger<PostDAL> _logger;
        public PostDAL(Neo4jContext context, ILogger<PostDAL> logger, IMapper mapper) : base(context, mapper)
        {
            _logger = logger;
        }

        public async Task<bool> ConfigPost(Guid postId, bool? isPublish = null, bool? isDelete = null, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Write.Merge($"(p:{PostSchema.POST_LABEL} {{ id: $id }})")
               .WithParam("id", postId)
               .OnCreate()
               .Set("p = $p1")
               .WithParam("p1", new
               {
                   id = postId,
                   isPublic = isPublish,
                   createdAt = DateTime.UtcNow,
               });
            if (isDelete.HasValue)
            {
                cypher = cypher.OnMatch()
                   .Set("p += $p2")
                   .WithParam("p2", new
                   {
                       deletedAt = DateTime.UtcNow,
                   })
                   .With("p");
            }
            else
            {
                cypher = cypher.OnMatch()
                   .Set("p += $p2")
                   .WithParam("p2", new
                   {
                       isPublic = isPublish,
                       updatedAt = DateTime.UtcNow,
                   })
                   .With("p");
            }

            var cypherResult = await cypher.Return<PostSchema>("p").ResultsAsync;
            var userSchema = cypherResult.FirstOrDefault();

            return userSchema?.CreatedAt != null;
        }

        public async Task<bool> CreateOrUpdatePost(PostDto postDto, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Write.Merge($"(p:{PostSchema.POST_LABEL} {{ id: $id }})")
               .WithParam("id", postDto.PostId)
               .OnCreate()
               .Set("p = $p1")
               .WithParam("p1", new
               {
                   id = postDto.PostId,
                   langId = postDto.LangId,
                   isPublic = postDto.IsPublic,
                   createdAt = DateTime.UtcNow,
               })
               .OnMatch()
               .Set("p += $p2")
               .WithParam("p2", new
               {
                   langId = postDto.LangId,
                   isPublic = postDto.IsPublic,
                   updatedAt = DateTime.UtcNow,
               })
               .With("p");
            cypher = cypher.Match($"(u: {UserSchema.USER_LABEL} {{ id : $userId }})")
                .WithParam("userId", postDto.UserId)
                .Merge($"(u)-[r:{RelationValues.HAS_POST}]->(p)")
                .With("p");

            var cypherResult = await cypher.Return<PostSchema>("p").ResultsAsync;
            var userSchema = cypherResult.FirstOrDefault();

            return userSchema?.CreatedAt != null;
        }

        public async Task<List<Guid>> FilterPostByLanguages(List<Guid> langIds, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Read
                 .Match($"(p: {PostSchema.POST_LABEL})")
                 .Where("p.deletedAt is null")
                 .AndWhere("p.isPublic = true")
                 .AndWhere($"p.langId IN $ids")
                 .WithParam("ids", langIds.ToArray())
                 .Return<Guid>("p.id");
            var cypherResult = await cypher.ResultsAsync;
            return cypherResult.ToList();
        }

        public async Task<List<Guid>> SuggestPostsAsync(Guid id, List<Guid> langIds, bool isOnlyFriend = false, bool isNewest = true, CancellationToken cancellationToken = default)
        {
            if (isOnlyFriend)
            {
                var fpcypher = _context.Cypher.Read
                            .Match($"(:{UserSchema.USER_LABEL} {{ id: $id }})-[:{RelationValues.HAS_FRIEND}]-(:{UserSchema.USER_LABEL})-[:{RelationValues.HAS_POST}]->(fp:{PostSchema.POST_LABEL})")
                            .WithParam("id", id)
                            .Where("fp.deletedAt is null")
                            .AndWhere("fp.isPublic = true")
                            .AndWhere($"fp.langId IN $ids")
                            .WithParam("ids", langIds.ToArray())
                            //.OrderBy("fp.createdAt")
                            .Return<Guid>("fp.id");
                return (await fpcypher.ResultsAsync).ToList();
            }
            var cypher = _context.Cypher.Read
                .Match($"(p: {PostSchema.POST_LABEL})")
                .Where("p.deletedAt is null")
                .AndWhere("p.isPublic = true")
                .AndWhere($"p.langId IN $ids")
                .WithParam("ids", langIds.ToArray());
                //.OrderByDescending("p.createdAt");

            var cypherResult = await cypher.Return<Guid>("p.id").ResultsAsync;
            return cypherResult.ToList();
        }
    }
}
