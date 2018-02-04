// Copyright (C) 2018  Samuel Fisher
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Text;
using Airlock.EntityFrameworkCore.Hive.Infrastructure.Internal;
using Airlock.EntityFrameworkCore.Hive.Query.ExpressionTranslators.Internal;
using Airlock.EntityFrameworkCore.Hive.Query.Sql.Internal;
using Airlock.EntityFrameworkCore.Hive.Storage.Internal;
using Airlock.EntityFrameworkCore.Hive.Update.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Airlock.EntityFrameworkCore.Hive.Infrastructure
{
    public static class HiveServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkHive(this IServiceCollection serviceCollection)
        {
            var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                          .TryAdd<IDatabaseProvider, DatabaseProvider<HiveOptionsExtension>>()
                          .TryAdd<IRelationalTypeMapper, HiveTypeMapper>()
                          .TryAdd<ISqlGenerationHelper, HiveSqlGenerationHelper>()
                          .TryAdd<IUpdateSqlGenerator, HiveUpdateSqlGenerator>()
                          .TryAdd<IModificationCommandBatchFactory, HiveModificationCommandBatchFactory>()
                          .TryAddProviderSpecificServices(b => b.TryAddScoped<IRelationalConnection, HiveRelationalConnection>())
                          .TryAdd<IMemberTranslator, HiveCompositeMemberTranslator>()
                          .TryAdd<ICompositeMethodCallTranslator, HiveCompositeMethodCallTranslator>()
                          .TryAdd<IQuerySqlGeneratorFactory, HiveQuerySqlGeneratorFactory>();

            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}
