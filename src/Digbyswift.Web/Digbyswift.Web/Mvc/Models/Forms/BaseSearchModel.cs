﻿using Microsoft.AspNetCore.Mvc;

namespace Digbyswift.Web.Mvc.Models.Forms
{
    public class BaseSearchModel
    {
        private int _page;
        [FromQuery(Name = "p")]
        public int Page
        {
            get => _page < 1 ? 1 : _page;
            set => _page = value;
        }

        private int _pageSize;
        [FromQuery(Name = "s")]
        public int PageSize
        {
            get => _pageSize < 1 ? 1 : _pageSize;
            set => _pageSize = value;
        }
    }
}
