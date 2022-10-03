using System.Linq.Dynamic.Core.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.GuestContacts.Queries;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BookingApp.Core.Tests.GuestContacts;

public class SearchGuestContactQueryHandlerTest : TestBase
{

    private readonly static Guest[] DataSeed = new[] {
        new Guest(new Guid("9144921c-d1b5-489d-9d52-99b53711cae6"), "Daniel Correia Carvalho", new List<GuestContact>() {
            new GuestContact(new Guid("022c1fa5-b35a-4ce3-980d-c6ac1f1d501b"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 1"),
            new GuestContact(new Guid("809fe7d7-a17d-4a1a-999b-a04101310173"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 2"),
            new GuestContact(new Guid("f349d753-7cd6-403a-b43f-3dccfd28fdbb"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 3"),
            new GuestContact(new Guid("e69dfdc5-53f8-477c-b136-d9e8ee2b444e"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 4"),
            new GuestContact(new Guid("18fbc275-ac9e-4d45-8005-39de72471f89"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 5"),
            new GuestContact(new Guid("779cb1da-936a-4f9c-8945-f78c4163c36f"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 6"),
            new GuestContact(new Guid("29c6de5c-e158-43b5-a6cf-ef3053be985b"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 6"),
            new GuestContact(new Guid("48b5a361-1432-4d3c-9a92-8be77d4db06f"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 7")
        }.AsReadOnly()),
        new Guest(new Guid("ea0202ae-72d3-492c-8996-086d861c249f"), "Julieta Alves Barros", new List<GuestContact>() {
            new GuestContact(new Guid("6bd44604-461a-4ab3-a21a-1652ef74109a"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 8"),
            new GuestContact(new Guid("2e2fb31c-e483-4cfc-b61b-a9f8d99b3651"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 9"),
            new GuestContact(new Guid("6ce96d77-cffa-4a6a-b70e-752b60471905"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 10"),
            new GuestContact(new Guid("c048f8c6-a5b3-452a-a7ec-d688fbefefce"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 11"),
            new GuestContact(new Guid("3fe757a5-7f1c-499d-ac17-71b7f281de9a"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 12"),
            new GuestContact(new Guid("a8c8ae7d-b686-4c8f-b6f6-5d589406f9dd"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 13"),
            new GuestContact(new Guid("03106226-f6b9-465e-8301-5d646325eeb4"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 14"),
            new GuestContact(new Guid("d6bb5c2a-4583-414f-bfbd-04c078f0c126"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 15")
        }.AsReadOnly()),
        new Guest(new Guid("31a7b83c-536f-4375-a3af-83861757c36e"), "Maria Azevedo Castro", new List<GuestContact>() {
            new GuestContact(new Guid("5bdccfe3-2b71-43bd-b8c0-a13faec440f8"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 16"),
            new GuestContact(new Guid("72f94fa5-2ebc-438f-a6fc-2c35981c5c6a"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 17"),
            new GuestContact(new Guid("672c1eb3-4e62-4fcb-83e4-f29f2ad28b68"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 18"),
            new GuestContact(new Guid("9aa47e79-f46f-4905-82dc-880c7472526a"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 19"),
            new GuestContact(new Guid("c5db67f1-bc39-4712-a86a-c9d176b2d76f"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 20"),
            new GuestContact(new Guid("7f7324fa-9be2-4f8f-a60f-3903260c7806"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 21"),
            new GuestContact(new Guid("75b1151b-2221-4455-a941-631b9d1d35a4"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 22"),
            new GuestContact(new Guid("0f607f22-82cb-451c-8076-4378f4a052f7"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 23")
        }.AsReadOnly()),
        new Guest(new Guid("154e7e8d-c778-47fe-bee8-ff3e3a4f3160"), "Eduardo Alves Almeida", new List<GuestContact>() {
            new GuestContact(new Guid("076e41ce-d04d-4e49-b6b4-56e6e8d4d5de"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 24"),
            new GuestContact(new Guid("709a18ca-1127-4ab7-a34b-f9843f1e6551"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 25"),
            new GuestContact(new Guid("fc4dfcd3-d6d9-45d7-8109-ff0a4d52876e"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 26"),
            new GuestContact(new Guid("3f9daac7-1f4d-4e08-b502-56e55abae09b"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 27"),
            new GuestContact(new Guid("7be04407-e912-4533-bd44-a8619f9c623b"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 28"),
            new GuestContact(new Guid("4f57030b-1ee0-4f43-b7b7-7ed37d04956f"), Guid.Empty, GuestContactTypeEnum.Phone, "Bla 29"),
            new GuestContact(new Guid("6f9c71c5-1c81-423c-bd24-50029d88be79"), Guid.Empty, GuestContactTypeEnum.Undefined, "Bla 30"),
            new GuestContact(new Guid("b904287b-8427-4111-a3ac-6c5614847084"), Guid.Empty, GuestContactTypeEnum.Email, "Bla 31")
        }.AsReadOnly())
    };

    public SearchGuestContactQueryHandlerTest() : base()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Guests.AddRange(DataSeed);
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task Should_Throw_ValidationException_If_GuestId_is_Empty()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(new SearchGuestContactQuery(Guid.Empty) { GuestId = Guid.Empty }, CancellationToken.None));
    }

    [Fact]
    public async Task Should_Get_a_full_Collection_of_Guests()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();
        var guest = DataSeed[2];
        var first = guest.Contacts.First();
        var last = guest.Contacts.Last();

        var cmd = new SearchGuestContactQuery(guest.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.Count().ShouldBe(guest.Contacts.Count());
        items.ShouldContain(q => q.Id == first.Id);
        items.ShouldContain(q => q.Id == last.Id);
    }

    [Fact]
    public async Task Should_Get_a_Paged_Collection_of_Guests()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();

        var guest = DataSeed[1];
        var cmd = new SearchGuestContactQuery(guest.Id) { Skip = 0, Take = 2 };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.Count().ShouldBe(2);
    }

    [Fact]
    public async Task Should_Get_a_Correct_Total_Count_on_Paged_Results()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();

        var guest = DataSeed[1];
        var cmd = new SearchGuestContactQuery(guest.Id) { Skip = 0, Take = 2 };
        var result = await handler.Handle(cmd, CancellationToken.None);

        result.TotalCount.ShouldBe(DataSeed[1].Contacts.Count());
    }

    [Fact]
    public async Task Should_Filter_by_Name()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();
        var guest = DataSeed[0];

        var cmd = new SearchGuestContactQuery(guest.Id) { Search = "Bla 3" };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.ShouldContain(q => q.Id == new Guid("f349d753-7cd6-403a-b43f-3dccfd28fdbb"));
    }

    [Fact]
    public async Task Should_Sort_by_Name()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();
        var guest = DataSeed[0];

        var cmd = new SearchGuestContactQuery(guest.Id) { SortBy = "value desc" };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.First().Id.ShouldBe(new Guid("48b5a361-1432-4d3c-9a92-8be77d4db06f"));
        items.Last().Id.ShouldBe(new Guid("022c1fa5-b35a-4ce3-980d-c6ac1f1d501b"));
    }

    [Fact]
    public async Task Should_Sort_by_Id()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();
        var guest = DataSeed[3];

        var cmd = new SearchGuestContactQuery(guest.Id) { SortBy = "id asc" };
        var result = await handler.Handle(cmd, CancellationToken.None);
        var items = await result.Items.ToListAsync();

        items.First().Id.ShouldBe(new Guid("076e41ce-d04d-4e49-b6b4-56e6e8d4d5de"));
        items.Last().Id.ShouldBe(new Guid("fc4dfcd3-d6d9-45d7-8109-ff0a4d52876e"));
    }

    [Fact]
    public async Task Should_Not_Sort_by_Invalid_Property()
    {
        var handler = _injector.GetRequiredService<SearchGuestContactQueryHandler>();
        var guest = DataSeed[0];

        var cmd = new SearchGuestContactQuery(guest.Id) { SortBy = "prilimpimpim asc" };
        await Should.ThrowAsync<ParseException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
