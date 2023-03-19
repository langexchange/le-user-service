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
                   isPublic = isPublish.Value,
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
                       isPublic = isPublish.Value,
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
    }
}
