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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace Airlock.EntityFrameworkCore.Hive.Query.ExpressionTranslators.Internal
{
    public class HiveStartsWithTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo MethodInfo
            = typeof(string).GetRuntimeMethod(nameof(string.StartsWith), new[] { typeof(string) });

        private static readonly MethodInfo Concat
            = typeof(string).GetRuntimeMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) });

        public Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (Equals(methodCallExpression.Method, MethodInfo))
            {
                var patternExpression = methodCallExpression.Arguments[0];
                
                // If the 'startsWith' value is a constant, produce:
                //   LIKE 'startsWith%'
                if (patternExpression is ConstantExpression constantExpression)
                {
                    if ((string)constantExpression.Value == string.Empty)
                        return Expression.Constant(true);

                    return new LikeExpression(methodCallExpression.Object,
                                              Expression.Constant(constantExpression.Value + "%"));
                }

                // Otherwise, produce:
                //   LIKE CONCAT(startsWith, '%')
                var startsWithExpression = new LikeExpression(methodCallExpression.Object,
                                                              new SqlFunctionExpression("CONCAT", typeof(string), new[]
                                                              {
                                                                  methodCallExpression.Arguments[0],
                                                                  Expression.Constant("%"),
                                                              }));

                return Expression.OrElse(startsWithExpression, Expression.Equal(patternExpression, Expression.Constant(string.Empty)));
            }

            return null;
        }
    }
}
