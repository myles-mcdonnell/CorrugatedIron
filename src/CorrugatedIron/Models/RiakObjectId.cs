﻿// Copyright (c) 2011 - OJ Reeves & Jeremiah Peschka
//
// This file is provided to you under the Apache License,
// Version 2.0 (the "License"); you may not use this file
// except in compliance with the License.  You may obtain
// a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System;
using CorrugatedIron.Converters;
using Newtonsoft.Json;

namespace CorrugatedIron.Models
{
    [JsonConverter(typeof(RiakObjectIdConverter))]
    public class RiakObjectId : IEquatable<RiakObjectId>
    {
        public string Bucket { get; set; }
        public string BucketType { get; set; }
        public string Key { get; set; }

        public RiakObjectId()
        {
        }

        public RiakObjectId(string[] objectId)
        {
            Bucket = objectId[0];
            Key = objectId[1];
        }


        public RiakObjectId(string bucket, string key)
        {
            Bucket = bucket;
            Key = key;
        }

        public RiakObjectId(string bucketType, string bucket, string key) : this (bucket, key)
        {
            BucketType = bucketType;
        }

        internal RiakLink ToRiakLink(string tag)
        {
            return new RiakLink(Bucket, Key, tag);
        }

        public bool Equals(RiakObjectId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Bucket, other.Bucket) && string.Equals(BucketType, other.BucketType) && string.Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RiakObjectId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Bucket != null ? Bucket.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (BucketType != null ? BucketType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Key != null ? Key.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(RiakObjectId left, RiakObjectId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RiakObjectId left, RiakObjectId right)
        {
            return !Equals(left, right);
        }
    }
}