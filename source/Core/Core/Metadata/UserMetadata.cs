﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */
using System;
using System.Linq;
using System.Collections.Generic;

namespace Thinktecture.IdentityManager
{
    public class UserMetadata
    {
        public static string UsernameType = Constants.ClaimTypes.Username;
        public static string PasswordType = Constants.ClaimTypes.Password;

        public bool SupportsCreate { get; set; }
        public bool SupportsDelete { get; set; }
        public bool SupportsClaims { get; set; }
        
        public IEnumerable<PropertyMetadata> Properties { get; set; }

        internal void Validate()
        {
            if (SupportsCreate)
            {
                //if (UsernameProperty == null)
                //{
                //    throw new InvalidOperationException("UsernameProperty required if SupportsCreate enabled");
                //}
                //if (PasswordProperty == null)
                //{
                //    throw new InvalidOperationException("PasswordProperty required if SupportsCreate enabled");
                //}
            }

            if (Properties == null) Properties = Enumerable.Empty<PropertyMetadata>();
            foreach (var prop in Properties) prop.Validate();
            
            var types = Properties.Select(x => x.Type).Distinct();
            if (types.Count() < Properties.Count())
            {
                var query =
                    from t in types
                    let props = (from p in Properties where p.Type == t select p)
                    where props.Count() > 1
                    select t;
                var names = query.Distinct().Aggregate((x, y) => x + ", " + y);
                throw new InvalidOperationException("Duplicate PropertyMetadata Types registered: " + names);
            }
        }
    }
}