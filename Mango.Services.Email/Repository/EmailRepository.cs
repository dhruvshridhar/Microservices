﻿using Mango.Services.Email.DbContexts;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
	{
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;
		public EmailRepository(DbContextOptions<ApplicationDbContext> dbContext)
		{
            _dbContext = dbContext;
		}

        public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
        {
            //Assuming I have magically implemented Email service
            //Now logging it

            var emailLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully"
            };

            await using var _db = new ApplicationDbContext(_dbContext);
            _db.emailLogs.Add(emailLog);
            await _db.SaveChangesAsync();
        }
    }
}
