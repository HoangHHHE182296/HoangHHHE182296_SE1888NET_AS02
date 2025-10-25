using BusinessLogic.DTOs.Requests;
using BusinessLogic.DTOs.Response;
using BusinessLogic.Rules;
using BusinessLogic.Validation;
using Core.Enums;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services {
    public class NewsArticleService {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly ITagRepository _tagRepository;
        private readonly NewsArticleValidator _newsArticleValidator;
        private readonly NewsArticleRules _newsArticleRules;

        public NewsArticleService(INewsArticleRepository newsArticleRepository, ISystemAccountRepository systemAccountRepository, ITagRepository tagRepository, NewsArticleValidator newsArticleValidator, NewsArticleRules newsArticleRules) {
            _newsArticleRepository = newsArticleRepository;
            _systemAccountRepository = systemAccountRepository;
            _tagRepository = tagRepository;
            _newsArticleValidator = newsArticleValidator;
            _newsArticleRules = newsArticleRules;
        }

        public async Task<IEnumerable<NewsArticleResponse>> SearchNewsArticleAsync(SearchNewsArticleRequest searchNewsArticleRequest) {
            bool? isActive = searchNewsArticleRequest.Status == null ? null : searchNewsArticleRequest.Status == Status.Active;

            var newsArticles = await _newsArticleRepository.SearchNewsArticleAsync(searchNewsArticleRequest.Keyword, searchNewsArticleRequest.Author, searchNewsArticleRequest.Category, isActive);

            var tasks = newsArticles.Select(async newsArticle => {
                var createdByTask = _systemAccountRepository.GetAccountByIdAsync((short)newsArticle.CreatedById);
                var updatedByTask = _systemAccountRepository.GetAccountByIdAsync((short)newsArticle.UpdatedById);

                var createdBy = await createdByTask;
                var updatedBy = await updatedByTask;

                return new NewsArticleResponse {
                    NewsArticleId = newsArticle.NewsArticleId,
                    NewsTitle = newsArticle.NewsTitle,
                    Headline = newsArticle.Headline,
                    CreatedDate = newsArticle.CreatedDate,
                    NewsContent = newsArticle.NewsContent,
                    Category = newsArticle.Category == null ? null : new CategoryResponse {
                        CategoryId = newsArticle.Category.CategoryId,
                        CategoryName = newsArticle.Category.CategoryName,
                        CategoryDescription = newsArticle.Category.CategoryDesciption,
                        Status = newsArticle.Category.IsActive != null && (bool)newsArticle.Category.IsActive
                            ? Status.Active
                            : Status.Inactive,
                    },
                    NewsStatus = newsArticle.NewsStatus != null && (bool)newsArticle.NewsStatus
                        ? Status.Active
                        : Status.Inactive,
                    CreatedBy = new SystemAccountResponse {
                        AccountId = createdBy.AccountId,
                        AccountName = createdBy.AccountName,
                    },
                    UpdatedBy = new SystemAccountResponse {
                        AccountId = updatedBy.AccountId,
                        AccountName = updatedBy.AccountName,
                    },
                    ModifiedDate = newsArticle.ModifiedDate
                };
            }).ToList();

            var result = await Task.WhenAll(tasks);
            return result;
        }

        public async Task AddNewsArticleAsync(CreateNewsArticleRequest createNewsArticleRequest) {
            _newsArticleValidator.ValidateForCreate(createNewsArticleRequest);
            await _newsArticleRules.CheckForCreate(createNewsArticleRequest);
            var tags = await _tagRepository.GetListTagByIdsAsync(createNewsArticleRequest.TagsId);

            await _newsArticleRepository.AddNewsArticleAsync(new NewsArticle {
                NewsTitle = createNewsArticleRequest.NewsTitle,
                Headline = createNewsArticleRequest.Headline,
                NewsContent = createNewsArticleRequest.NewsContent,
                CategoryId = (short)createNewsArticleRequest.CategoryId,
                NewsSource = createNewsArticleRequest.NewsSource,
                NewsStatus = true,
                Tags = (ICollection<Tag>)tags
            });
        }

        public async Task UpdateNewsArticleAsync(UpdateNewsArticleRequest updateNewsArticleRequest) {
            _newsArticleValidator.ValidateForUpdate(updateNewsArticleRequest);
            await _newsArticleRules.CheckForUpdate(updateNewsArticleRequest);

            var tags = await _tagRepository.GetListTagByIdsAsync(updateNewsArticleRequest.TagsId);

            await _newsArticleRepository.UpdateNewsArticleAsync(new NewsArticle {
                NewsArticleId = updateNewsArticleRequest.NewsArticleId,
                NewsTitle = updateNewsArticleRequest.NewsTitle,
                Headline = updateNewsArticleRequest.Headline,
                NewsContent = updateNewsArticleRequest.NewsContent,
                CategoryId = (short)updateNewsArticleRequest.CategoryId,
                NewsSource = updateNewsArticleRequest.NewsSource,
                Tags = (ICollection<Tag>)tags
            });
        }

        public async Task DeleteNewsArticleAsync(string newsArticleId) {

            await _newsArticleRepository.DeleteNewsArticleAsync(newsArticleId);
        }
    }
}
