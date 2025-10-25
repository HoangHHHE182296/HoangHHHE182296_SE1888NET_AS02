using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Requests {
    public class SearchNewsArticleRequest {
        public string? Keyword { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public Status? Status { get; set; }
    }

    public class CreateNewsArticleRequest {
        [Required]
        public string? NewsTitle { get; set; }

        [Required]
        public string? Headline { get; set; }

        [Required]
        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<int>? TagsId { get; set; }
    }

    public class UpdateNewsArticleRequest {
        public string NewsArticleId { get; set; }

        [Required]
        public string? NewsTitle { get; set; }

        [Required]
        public string? Headline { get; set; }

        [Required]
        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public Status? Status { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<int>? TagsId { get; set; }
    }
}
