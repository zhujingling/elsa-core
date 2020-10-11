﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Indexes;
using Elsa.Models;
using Elsa.Services;
using YesSql;

namespace Elsa.Queries
{
    public class WorkflowInstanceByDefinitionAndStatusQuery : ICompiledQuery<WorkflowInstance>
    {
        public string WorkflowDefinitionId { get; }
        public WorkflowStatus Status { get; }

        public WorkflowInstanceByDefinitionAndStatusQuery(string workflowDefinitionId, WorkflowStatus status)
        {
            WorkflowDefinitionId = workflowDefinitionId;
            Status = status;
        }

        public Expression<Func<IQuery<WorkflowInstance>, IQuery<WorkflowInstance>>> Query() => query =>
            query.With<WorkflowInstanceIndex>().Where(
                x => x.WorkflowDefinitionId == WorkflowDefinitionId && x.WorkflowStatus == Status);
    }

    public static class WorkflowInstanceByDefinitionAndStatusQueryWorkflowInstanceManagerExtensions
    {
        public static Task<IEnumerable<WorkflowInstance>> ListByDefinitionAndStatusAsync(
            this IWorkflowInstanceManager manager,
            string workflowDefinitionId,
            WorkflowStatus workflowStatus,
            CancellationToken cancellationToken = default) =>
            manager
                .ExecuteQuery(
                    new WorkflowInstanceByDefinitionAndStatusQuery(workflowDefinitionId, workflowStatus))
                .ListAsync();
    }
}