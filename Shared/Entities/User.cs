using System;
using System.Collections.Generic;

namespace BlazorBattles.Shared.Entities
{
    public record User
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string UserName { get; init; }
        public byte[] PasswordHash { get; init; }
        public byte[] PasswordSalt { get; init; }
        public int Bananas { get; init; }
        public DateTime DateOfBirth { get; init; }
        public bool IsConfirmed { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime DateCreated { get; init; } = DateTime.Now;
    }
    
}