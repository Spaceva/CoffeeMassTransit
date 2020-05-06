using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebinarMassTransit.Contracts;

namespace WebinarMassTransit.Core.DAL
{
    public class CoffeeDapperRepository : ICoffeeRepository
    {
        private readonly SqlConnectionFactory sqlConnectionFactory;

        public CoffeeDapperRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public void Create(Guid coffeeId, CoffeeType coffeeType, bool noTopping)
        {
            var query = @$"INSERT INTO   [Coffees] ({nameof(Coffee.Id)}, {nameof(Coffee.Type)}, {nameof(Coffee.Done)}) 
                        OUTPUT          Inserted.Id
                        VALUES          (@{nameof(coffeeId)}, @{nameof(coffeeType)}, @{nameof(noTopping)})";
            using var conn = sqlConnectionFactory.Create();
            conn.ExecuteScalar(query, new { coffeeId, coffeeType, noTopping });
        }

        public void AddToppings(Guid coffeeId, IReadOnlyCollection<Topping> toppings)
        {
            var toppingsList = (toppings?.Count > 0) ? string.Join(",", toppings) : null;
            var query = @$"UPDATE [Coffees] SET {nameof(Coffee.Toppings)} = @{nameof(toppingsList)}, {nameof(Coffee.Done)} = 1";
            using var conn = sqlConnectionFactory.Create();
            conn.Execute(query, new { coffeeId, toppingsList });
        }

        public Coffee Get(Guid coffeeId)
        {
            var query = $"SELECT * FROM [Coffees] WHERE [{nameof(Coffee.Id)}] = @{nameof(coffeeId)}";
            using var conn = sqlConnectionFactory.Create();
            var coffeeDb = conn.QuerySingleOrDefault<CoffeeDbModel>(query, new { coffeeId });
            return GetCoffeeFromDbModel(coffeeDb);
        }

        public IReadOnlyCollection<Coffee> GetAll()
        {
            var query = $"SELECT * FROM [Coffees]";
            using var conn = sqlConnectionFactory.Create();
            return conn.Query<CoffeeDbModel>(query).Select(GetCoffeeFromDbModel).ToArray();
        }

        private Coffee GetCoffeeFromDbModel(CoffeeDbModel coffeeDb)
        {
            return new Coffee { Id = coffeeDb.Id, Done = coffeeDb.Done, Type = coffeeDb.Type, Toppings = coffeeDb.Toppings?.Split(",").Select(t => Enum.Parse<Topping>(t))?.ToList() ?? new List<Topping>() };
        }
    }
}
