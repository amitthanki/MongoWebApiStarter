﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using MongoWebApiStarter.Data.Entities;
using MongoWebApiStarter.Data.Repos;
using System;
using System.Linq;

namespace MongoWebApiStarter.Test
{
    [TestClass]
    public class EmailTest
    {
        private static readonly string email = $"{Guid.NewGuid().ToString()}@email.com";
        private static readonly EmailRepo repo = new EmailRepo();

        [TestMethod]
        public void saving_an_email()
        {
            for (int i = 1; i <= 10; i++)
            {
                repo.Save(new EmailMessage
                {
                    FromEmail = email,
                    FromName = "From Me",
                    Subject = "Test Email",
                    ToEmail = email,
                    ToName = email,
                    BodyHTML = "this is a test email message"
                });
            }
        }

        [TestMethod]
        public void fetching_next_batch_of_emails()
        {
            repo.FetchNextBatch(10).Count.Should().Be(10);
        }

        [TestMethod]
        public void marking_email_as_sent()
        {
            repo.Save(new EmailMessage
            {
                FromEmail = email,
                FromName = "From Me",
                Subject = "Test Email",
                ToEmail = email,
                ToName = email,
                BodyHTML = "this is a test email message"
            });

            var id = repo.FetchNextBatch(1).Single().ID;
            repo.MarkAsSent(id);

            DB.Find<EmailMessage>()
              .Match(e => e.ID == id)
              .Project(e => new EmailMessage { Sent = e.Sent })
              .Execute()
              .Single()
              .Sent.Should().BeTrue();
        }
    }
}
