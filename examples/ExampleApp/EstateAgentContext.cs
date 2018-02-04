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
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp
{
    class EstateAgentContext : DbContext
    {
        public EstateAgentContext(DbContextOptions<EstateAgentContext> contextOptions)
            : base(contextOptions)
        {
        }

        public DbSet<Property> Properties { get; set; }

        public DbSet<PropertyViewing> PropertyViewings { get; set; }
    }

    public class Property
    {
        [Key]
        public int Id { get; set; }

        public DateTime Added { get; set; }

        public string ShortAddress { get; set; }

        public double Price { get; set; }

        public bool IsAvailable { get; set; }
    }

    public class PropertyViewing
    {
        [Key]
        public int Id { get; set; }

        public int PropertyId { get; set; }

        public string ClientName { get; set; }

        public DateTime ViewingTime { get; set; }
    }
}
