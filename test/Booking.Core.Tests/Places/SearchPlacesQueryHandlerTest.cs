using System.Linq.Dynamic.Core.Exceptions;
using Booking.Core.Data;
using Booking.Core.Places;
using Booking.Core.Places.Models;
using Booking.Core.Places.Queries;
using Booking.Core.Tests.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Places;

public class SearchPlacesQueryHandlerTest : BaseTest
{
    private readonly static Place[] DataSeed = new[] {
        new Place() { Id = new Guid("434db615-04f4-43ff-b881-415903111def"), Address = "Jona's Place 1", Name = "Jonas 1" },
        new Place() { Id = new Guid("6b6ace9f-4c61-414a-a5a9-5b5822140b98"), Address = "Jona's Place 2", Name = "Jonas 2" },
        new Place() { Id = new Guid("a84bfefc-ae76-405e-91fe-a2b21a4f254c"), Address = "Jona's Place 3", Name = "Jonas 3" },
        new Place() { Id = new Guid("31dcf215-91c4-4ba9-979b-bd8a290e8a83"), Address = "Jona's Place 4", Name = "Enzo Oliveira Fernandes" },
        new Place() { Id = new Guid("f0ab43d1-d065-4093-9dc4-46eda747acad"), Address = "Jona's Place 5", Name = "Cauã Cavalcanti Almeida" },
        new Place() { Id = new Guid("05c8543e-306d-42b2-920c-38c0c512726a"), Address = "Jona's Place 6", Name = "Daniel Correia Carvalho" },
        new Place() { Id = new Guid("ea0202ae-72d3-492c-8996-086d861c249f"), Address = "Jona's Place 7", Name = "Julieta Alves Barros" },
        new Place() { Id = new Guid("31a7b83c-536f-4375-a3af-83861757c36e"), Address = "Jona's Place 8", Name = "Maria Azevedo Castro" },
        new Place() { Id = new Guid("154e7e8d-c778-47fe-bee8-ff3e3a4f3160"), Address = "Jona's Place 9", Name = "Eduardo Alves Almeida" },
        new Place() { Id = new Guid("db315b82-158a-42a8-8c77-ef0e8bbea030"), Address = "Jona's Place 10", Name = "Rafael Fernandes Araujo" },
        new Place() { Id = new Guid("2ae5edf6-4549-488b-a2b8-b31a7b3bc657"), Address = "Jona's Place 11", Name = "Marisa Barros Pereira" },
        new Place() { Id = new Guid("0bb052f1-df89-4cd0-8235-901f9cd6cc4f"), Address = "Jona's Place 12", Name = "Kaua Dias Silva" },
        new Place() { Id = new Guid("33a6a7a8-a5ca-44d5-90a9-f685ed5db9ae"), Address = "Jona's Place 13", Name = "Isabella Cunha Rocha" },
        new Place() { Id = new Guid("f31fcf02-c090-44ac-98ff-e8605a219de6"), Address = "Jona's Place 14", Name = "Leila Silva Santos" },
        new Place() { Id = new Guid("f142ef36-27d3-40a2-92c2-e1512ca6fb80"), Address = "Jona's Place 15", Name = "Eduardo Oliveira Ribeiro" },
        new Place() { Id = new Guid("f20ededa-5e9b-4e87-ae10-0f650ab7dada"), Address = "Jona's Place 16", Name = "Luís Barbosa Goncalves" },
        new Place() { Id = new Guid("33c2653c-bbee-40c6-ac9d-52ef3508fa6c"), Address = "Jona's Place 17", Name = "Kauan Souza Sousa" },
        new Place() { Id = new Guid("9a4c7c20-3d32-41ff-972f-24a5e1690dde"), Address = "Jona's Place 18", Name = "André Alves Ferreira" },
        new Place() { Id = new Guid("28843ed3-5ac1-4857-be3a-84bbed41d283"), Address = "Jona's Place 19", Name = "Carlos Fernandes Lima" },
        new Place() { Id = new Guid("331f7196-e459-43b9-af1c-c69cf7d95e9f"), Address = "Jona's Place 20", Name = "Vitor Carvalho Melo" },
        new Place() { Id = new Guid("ba51093c-1030-483f-bbb8-e2590f9c2647"), Address = "Jona's Place 21", Name = "Victor Dias Pereira" }
    };

    public SearchPlacesQueryHandlerTest() : base()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Places.AddRange(DataSeed);
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task Should_Get_a_full_Collection_of_Places()
    {
        var handler = _injector.GetRequiredService<SearchPlaceQueryHandler>();
        var first = DataSeed.First();
        var last = DataSeed.Last();

        var cmd = new SearchPlaceQuery();
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.Count().ShouldBe(DataSeed.Length);
        items.ShouldContain(q => q.Id == first.Id);
        items.ShouldContain(q => q.Id == last.Id);
    }

    [Fact]
    public async Task Should_Get_a_Paged_Collection_of_Places()
    {
        var handler = _injector.GetRequiredService<SearchPlaceQueryHandler>();
        
        var cmd = new SearchPlaceQuery() { Skip = 0, Take = 2 };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.Count().ShouldBe(2);
    }

    [Fact]
    public async Task Should_Get_a_Correct_Total_Count_on_Paged_Results()
    {
        var handler = _injector.GetRequiredService<SearchPlaceQueryHandler>();
        var first = DataSeed.First();
        var last = DataSeed.Last();

        var cmd = new SearchPlaceQuery() { Skip = 0, Take = 4 };
        var result = await handler.Handle(cmd, CancellationToken.None);

        result.TotalCount.ShouldBe(DataSeed.Length);
    }

    [Fact]
    public async Task Should_Filter_by_Name()
    {
        var handler = _injector.GetRequiredService<SearchPlaceQueryHandler>();
        var first = DataSeed.First();
        var last = DataSeed.Last();

        var cmd = new SearchPlaceQuery() { Search = "Jonas" };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.ShouldContain(q => q.Id == new Guid("434db615-04f4-43ff-b881-415903111def"));
        items.ShouldContain(q => q.Id == new Guid("6b6ace9f-4c61-414a-a5a9-5b5822140b98"));
        items.ShouldContain(q => q.Id == new Guid("a84bfefc-ae76-405e-91fe-a2b21a4f254c"));
    }

    [Fact]
    public async Task Should_Sort_by_Name()
    {
        var handler = _injector.GetRequiredService<SearchPlaceQueryHandler>();
        var first = DataSeed.First();
        var last = DataSeed.Last();

        var cmd = new SearchPlaceQuery() { SortBy = "name desc" };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.First().Id.ShouldBe(new Guid("331f7196-e459-43b9-af1c-c69cf7d95e9f"));
        items.Last().Id.ShouldBe(new Guid("9a4c7c20-3d32-41ff-972f-24a5e1690dde"));
    }

    [Fact]
    public async Task Should_Sort_by_Id()
    {
        var handler = _injector.GetRequiredService<SearchPlaceQueryHandler>();
        var first = DataSeed.First();
        var last = DataSeed.Last();

        var cmd = new SearchPlaceQuery() { SortBy = "id asc" };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.First().Id.ShouldBe(new Guid("05c8543e-306d-42b2-920c-38c0c512726a"));
        items.Last().Id.ShouldBe(new Guid("f31fcf02-c090-44ac-98ff-e8605a219de6"));
    }

    [Fact]
    public async Task Should_Not_Sort_by_Invalid_Property()
    {
        var handler = _injector.GetRequiredService<SearchPlaceQueryHandler>();
        var first = DataSeed.First();
        var last = DataSeed.Last();

        var cmd = new SearchPlaceQuery() { SortBy = "prilimpimpim asc" };
        await Should.ThrowAsync<ParseException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
