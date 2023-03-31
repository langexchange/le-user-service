﻿using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Neo4jData.DALs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class VocabService : IVocabService
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;
        private readonly IVocabPackageDAL _vocabPackageDAL;

        public VocabService(LanggeneralDbContext context, IMapper mapper, IVocabPackageDAL vocabPackageDAL)
        {
            _context = context;
            _mapper = mapper;
            _vocabPackageDAL = vocabPackageDAL;
        }

        public async Task<Guid> CloneVocabularyPackageAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId && x.IsRemoved == false);
            if(vocabPackage == null)
                return Guid.Empty;

            var newVocabPackage = vocabPackage;
            newVocabPackage.Packageid = Guid.NewGuid();
            newVocabPackage.Userid = userId;
            newVocabPackage.IsPublic = true;
            newVocabPackage.IsRemoved = false;
            newVocabPackage.CreatedAt = DateTime.UtcNow;
            newVocabPackage.UpdatedAt = null;
            newVocabPackage.DeletedAt = null;
            _context.Vocabpackages.Add(newVocabPackage);
            await _context.SaveChangesAsync();

            var vocabDto = _mapper.Map<VocabularyPackageDto>(newVocabPackage);
            await _vocabPackageDAL.CreateOrUpdateVocabPackageAsync(vocabDto, cancellationToken);

            return newVocabPackage.Packageid;
        }

        public async Task<Guid> CreateOrUpdateVocabularyPackageAsync(VocabularyPackageDto dto, CancellationToken cancellationToken = default)
        {
            dto.PackageId = Guid.NewGuid();
            var vocabPackage = _mapper.Map<Vocabpackage>(dto);

            _context.Vocabpackages.Add(vocabPackage);
            await _context.SaveChangesAsync();

            //save in neo4j
            await _vocabPackageDAL.CreateOrUpdateVocabPackageAsync(dto, cancellationToken);

            return vocabPackage.Packageid;
        }

        public async Task<UserVocabPackageDto> GetVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default)
        {
            //var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId && x.IsRemoved == false);
            //if(vocabPackage == null)
            //    return null;
            //return _mapper.Map<VocabularyPackageDto>(vocabPackage);
            var dto = await _vocabPackageDAL.GetVocabularyPackageAsync(packageId, cancellationToken);
            return dto;
        }

        public async Task<UserVocabPackageDto> GetVocabularyPackagesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            //var vocabPackages = await _context.Vocabpackages.Where(x => x.Userid == userId && x.IsRemoved == false).ToListAsync();
            //if (vocabPackages == null)
            //    return null;
            //return _mapper.Map<List<UserVocabPackageDto>>(vocabPackages);
            var dto = await _vocabPackageDAL.GetVocabularyPackageByUserAsync(userId, cancellationToken);
            return dto;
        }

        public async Task<bool> IsBelongToUser(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Userid == userId && x.Packageid == packageId);
            if (vocabPackage == null)
                return false;
            return true;
        }

        public async Task SetVocabularyPackageState(Guid packageId, VocabPackageState state, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId);
            if (vocabPackage == null)
                return;

            switch (state)
            {
                case VocabPackageState.Publish:
                    vocabPackage.IsPublic = true;
                    break;
                case VocabPackageState.Private:
                    vocabPackage.IsPublic = false;
                    break;
                case VocabPackageState.Delete:
                    vocabPackage.IsRemoved = true;
                    vocabPackage.DeletedAt = DateTime.UtcNow;
                    break;
                default:
                    return;
            }
            _context.Update(vocabPackage);
            _context.SaveChanges();
        }

        public async Task<UserVocabPackageDto> SuggestVocabularyPackagesAsync(Guid userId, string termLocale, string defineLocale, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
