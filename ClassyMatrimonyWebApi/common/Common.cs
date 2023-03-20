using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MohsyWebApi
{
    public class PaginatedResult<T>
    {
        public T result { get; set; }
        public Pagination pagination { get; set; }

        public LeadsCountResult leadsCountResult { get; set; }

        public PaginatedResult(T result, Pagination pagination, LeadsCountResult leadsCountResult = null)
        {
            this.result = result;
            this.pagination = pagination;
            this.leadsCountResult = leadsCountResult;
        }
    }


    public partial class LeadsCountResult
    {
        public LeadsCountResult(int OpenLeadsCount, int TodayFollowupCount, int FollowupCount, int PromiseToPayCount, int WalkinCount, 
            int LeadClosedCount, int TotalAllCount, int callReachableCount, int callNotReachableCount,
            decimal targetAmount, decimal remainingAmount, int notconnectedCount, int notInterestedCount, int rNRCount, int switchedOffCount, int marriageFixedCount)
        {
            this.OpenLeadsCount = OpenLeadsCount;
            this.TodayFollowupCount = TodayFollowupCount;
            this.FollowupCount = FollowupCount;
            this.PromiseToPayCount = PromiseToPayCount;
            this.WalkinCount = WalkinCount;
            this.LeadClosedCount = LeadClosedCount;
            this.TotalAllCount = TotalAllCount;
            this.CallReachableCount = callReachableCount;
            this.CallNotReachableCount = callNotReachableCount;
            this.TargetAmount = targetAmount;
            this.RemainingAmount = remainingAmount;

            this.NotconnectedCount = notconnectedCount;
            this.NotInterestedCount = notInterestedCount;
            this.RNRCount = rNRCount;
            this.SwitchedOffCount = switchedOffCount;
            this.MarriageFixedCount = marriageFixedCount;
        }
        public int OpenLeadsCount { get; set; }
        public int TodayFollowupCount { get; set; }
        public int FollowupCount { get; set; }
        public int PromiseToPayCount { get; set; }
        public int WalkinCount { get; set; }
        public int LeadClosedCount { get; set; }
        public int TotalAllCount { get; set; }
        public int CallReachableCount { get; set; }
        public int CallNotReachableCount { get; set; }
        public decimal? TargetAmount { get; set; }
        public decimal? RemainingAmount { get; set; }

        public int NotconnectedCount { get; set; }
        public int NotInterestedCount { get; set; }
        public int RNRCount { get; set; }
        public int SwitchedOffCount { get; set; }
        public int MarriageFixedCount { get; set; }
    }

    public class Pagination
    {
        public Pagination(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}