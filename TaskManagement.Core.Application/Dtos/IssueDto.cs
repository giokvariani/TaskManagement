﻿using TaskManagement.Core.Domain.Enums;

namespace TaskManagement.Core.Application.Dtos
{
    public class IssueDto : CreateIssueDto
    {
        public int ReporterId { get; set; }
        public IssueStatusType Status { get; set; }
    }
}
