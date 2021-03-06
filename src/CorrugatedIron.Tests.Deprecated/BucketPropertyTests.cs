﻿using System;
using System.Linq;
using CorrugatedIron.Models;
using CorrugatedIron.Models.Search;
using CorrugatedIron.Tests.Extensions;
using CorrugatedIron.Tests.Live.LiveRiakConnectionTests;
using NUnit.Framework;

namespace CorrugatedIron.Tests.Live.Deprecated
{
    [TestFixture()]
    public class WhenDealingWithBucketProperties : LiveRiakConnectionTestBase
    {
        // use the one node configuration here because we might run the risk
        // of hitting different nodes in the configuration before the props
        // are replicated to other nodes.
        public WhenDealingWithBucketProperties()
            :base("riak1NodeConfiguration")
        {
        }

        [Test()]
        public void SettingLegacySearchOnRiakBucketMakesBucketSearchable()
        {
            var bucket = Guid.NewGuid().ToString();
            var key = Guid.NewGuid().ToString();
            var props = Client.GetBucketProperties(bucket).Value;
            props.SetLegacySearch(true);

            var setResult = Client.SetBucketProperties(bucket, props);
            setResult.IsSuccess.ShouldBeTrue(setResult.ErrorMessage);

            var obj = new RiakObject(bucket, key, new { name = "OJ", age = 34 });
            var putResult = Client.Put(obj);
            putResult.IsSuccess.ShouldBeTrue(putResult.ErrorMessage);

            var q = new RiakFluentSearch(bucket, "name")
                .Search("OJ")
                .And("age", "34")
                .Build();

            var search = new RiakSearchRequest
            {
                Query = q
            };

            var searchResult = Client.Search(search);
            searchResult.IsSuccess.ShouldBeTrue(searchResult.ErrorMessage);
            searchResult.Value.NumFound.ShouldEqual(1u);
            searchResult.Value.Documents[0].Fields.Count.ShouldEqual(3);
            searchResult.Value.Documents[0].Fields.First(x => x.Key == "id").Value.ShouldEqual(key);
        }
    }
}
