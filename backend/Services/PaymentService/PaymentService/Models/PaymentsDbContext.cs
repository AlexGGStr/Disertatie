using Microsoft.EntityFrameworkCore;

namespace PaymentService.Models;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }
}