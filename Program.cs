using HoneyRaesAPI.Models;
using HoneyRaesAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
List<Customer> customers = new List<Customer>
{
    new Customer()
     {
        Id = 1,
        Name = "Glidden Masters",
        Address = "2802 Zula Locks Dr"
    },
    new Customer()
    {
        Id = 2,
        Name = "Filipe Gonzaga",
        Address = "56849 Fadel Gateway"
    },
    new Customer()
    {
        Id = 3,
        Name = "Roger Talos",
        Address = "7346 Ritchie Road"
    },
    new Customer()
    {
        Id = 4,
        Name = "Zelda Reigns Cobbler",
        Address = "123 Fake St"
    },
    new Customer()
    {
        Id = 5,
        Name = "Juan Carlos Wong",
        Address = "888 Panama Rd"
    },
    new Customer()
    {
        Id = 6,
        Name = "Griper Bunson",
        Address = "1244 Carlitos Way"
    }
};

List<Employee> employees = new List<Employee>
{
    new Employee()
    {
        Id = 1,
        Name = "Willy Bender",
        Specialty = "Macs & PCs"
    },
    new Employee()
    {
        Id = 2,
        Name = "Cordon Blue",
        Specialty = "Viruses & Malware"
    },
    new Employee()
    {
        Id = 3,
        Name = "Bunson HoneyDew",
        Specialty = "Science"
    },
    new Employee()
    {
        Id = 4,
        Name = "Beeker",
        Specialty = "Beeping"
    },
    new Employee()
    {
        Id = 5,
        Name = "Rick",
        Specialty = "Drinking"
    }

};

List<ServiceTicket> serviceTickets = new List<ServiceTicket>
{
    new ServiceTicket()
    {
        Id = 1,
        CustomerId = 3,
        EmployeeId = 2,
        Description = "This is a ticket",
        Emergency = false,
        DateCompleted = new DateTime(2023, 07, 31)
    },
    new ServiceTicket()
    {
        Id = 2,
        CustomerId = 3,
        EmployeeId = 3,
        Description = "This is a ticket",
        Emergency = false,
        DateCompleted = new DateTime(2023, 08, 16)
    },
    new ServiceTicket()
    {
        Id = 3,
        CustomerId = 1,
        EmployeeId = 2,
        Description = "This is a ticket",
        Emergency = true,
        DateCompleted = null
    },
    new ServiceTicket()
    {
        Id = 4,
        CustomerId = 1,
        EmployeeId = 2,
        Description = "This is a ticket",
        Emergency = false,
        DateCompleted = null
    },
    new ServiceTicket()
    {
        Id = 5,
        CustomerId = 1,
        EmployeeId = 1,
        Description = "This is a ticket",
        Emergency = true,
        DateCompleted = new DateTime(2024, 08, 05)
    },
    new ServiceTicket()
    {
        Id = 6,
        CustomerId = 4,
        EmployeeId = 3,
        Description = "Gerbil in Printer",
        Emergency = true,
        DateCompleted = new DateTime(2024, 08, 05)
    },
    new ServiceTicket()
    {
        Id = 7,
        CustomerId = 5,
        EmployeeId = 4,
        Description = "Ipod stuck on nickelback",
        Emergency = true,
        DateCompleted = new DateTime(2024, 08, 01)
    },
      new ServiceTicket()
    {
        Id = 8,
        CustomerId = 1,
        Description = "hair dryer won't connect to wifi",
        Emergency = false
    }
};

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();






// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/servicetickets", () =>
{
    return serviceTickets.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        Description = t.Description,
        Emergency = t.Emergency,
        DateCompleted = t.DateCompleted
    });
});
app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);

    if (serviceTicket == null)
    {
        return Results.NotFound();
    }

    Employee employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);
    return Results.Ok(new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        EmployeeId = serviceTicket.EmployeeId,
        Employee = employee == null ? null : new EmployeeDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Specialty = employee.Specialty
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    });
});
app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{

    // Get the customer data to check that the customerid for the service ticket is valid
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);

    // if the client did not provide a valid customer id, this is a bad request
    if (customer == null)
    {
        return Results.BadRequest();
    }

    // creates a new id (SQL will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);

    // Created returns a 201 status code with a link in the headers to where the new resource can be accessed
    return Results.Created($"/servicetickets/{serviceTicket.Id}", new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency
    });

});
app.MapDelete("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }

    serviceTickets.Remove(serviceTicket);
    return Results.NoContent();
});
app.MapPut("/servicetickets/{id}", (int id, ServiceTicket serviceTicket) =>
{
    ServiceTicket ticketToUpdate = serviceTickets.FirstOrDefault(st => st.Id == id);

    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    if (id != serviceTicket.Id)
    {
        return Results.BadRequest();
    }

    ticketToUpdate.CustomerId = serviceTicket.CustomerId;
    ticketToUpdate.EmployeeId = serviceTicket.EmployeeId;
    ticketToUpdate.Description = serviceTicket.Description;
    ticketToUpdate.Emergency = serviceTicket.Emergency;
    ticketToUpdate.DateCompleted = serviceTicket.DateCompleted;

    return Results.NoContent();
});
app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    serviceTicket.DateCompleted = DateTime.Today;

});
app.MapGet("/employees", () =>
{
    return employees.Select(e => new EmployeeDTO
    {
        Id = e.Id,
        Name = e.Name,
        Specialty = e.Specialty
    });
});
app.MapGet("/customers", () =>
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    });
});
app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    List<ServiceTicket> tickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();
    return Results.Ok(new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
});


app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(st => st.Id == id);

    return new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address
    };
});

app.MapGet("/servicetickets/emergencies", () =>
{
    List<ServiceTicket> emergencyTickets = serviceTickets
    .Where(s => s.Emergency == true)
    .Where(s => s.DateCompleted == null)
    .ToList();

    return emergencyTickets.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        Description = t.Description,
        Emergency = t.Emergency
    });

});

app.MapGet("/servicetickets/unassigned", () =>
{
    List<ServiceTicket> unassignedTickets = serviceTickets
    .Where(s => s.EmployeeId == null)
    .ToList();

    return unassignedTickets.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        Description = t.Description,
        Emergency = t.Emergency
    });

});

app.MapGet("/employees/available", () =>
{
    List<Employee> availableBois = new List<Employee>();
    int count = 0;
    foreach (Employee e in employees)
    {
        List<ServiceTicket> employeeTickets = serviceTickets
        .Where(st => st.EmployeeId == e.Id)
        .ToList();
        foreach (ServiceTicket st in employeeTickets)
        {
            if (st.DateCompleted == null)
            {
                count += 1;
            }

        }
        if (count == 0)
        {
            availableBois.Add(e);
        }
        else { count = 0; }
    }
    return availableBois.Select(e => new EmployeeDTO
    {
        Id = e.Id,
        Name = e.Name,
        Specialty = e.Specialty
    });
});

app.MapGet("/customers/inactive", () =>
{
    List<Customer> inactiveCustomers = customers;
    foreach (Customer c in customers)
    {
        List<ServiceTicket> customerTickets = serviceTickets
        .Where(st => st.CustomerId == c.Id).ToList();
        foreach (ServiceTicket st in customerTickets)
        {
            if (st.DateCompleted >= DateTime.Now.AddYears(-1))
            {
                inactiveCustomers.Remove(c);
            }
        }
    }
    return Results.Ok(inactiveCustomers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    }));

});

app.MapGet("/employees/{id}/customers", (int id) =>
{
    Employee employee = employees.Where(e => e.Id == id).FirstOrDefault();
    EmployeeDTO eDTO = new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty,
        Customers = new List<CustomerDTO>()
    };
    foreach (ServiceTicket tick in serviceTickets)
    {
        if (tick.EmployeeId == id)
        {
            Customer c = customers.Where(c => c.Id == tick.CustomerId).FirstOrDefault();
            CustomerDTO employeeCustomer = new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address
            };
            eDTO.Customers.Add(employeeCustomer);

        }
    }
    return Results.Ok(eDTO)
    ;
});

app.MapGet("employees/ofthemonth", () =>
{
    List<ServiceTicket> ticketsOfTheMonth = new List<ServiceTicket>();
    foreach (ServiceTicket ticket in serviceTickets)
    {
        if (ticket.DateCompleted != null)
        {
            String dateString = ticket.DateCompleted.ToString();
            String month = dateString.Split('/')[0];
            int monthInt = Int32.Parse(month);
            if (monthInt == DateTime.Today.Month - 1)
            {
                ticketsOfTheMonth.Add(ticket);
            }
        }
    };
    int maxcount = 0;
    int element_having_max_freq = 0;
    foreach (ServiceTicket tick in ticketsOfTheMonth)
    {
        int count = 0;
        foreach (Employee employee in employees)
        {
            if (tick.EmployeeId == employee.Id)
            {
                count++;
            }
            if (count > maxcount)
            {
                maxcount = count;
                element_having_max_freq = employee.Id;
            };
        };
    };
    return Results.Ok(employees.Where(e => e.Id == element_having_max_freq)
    .Select(e => new EmployeeDTO
    {
        Id = e.Id,
        Name = e.Name,
        Specialty = e.Specialty
    }));



});

app.MapGet("tickets/bydate", () =>
{
    var ticketsByDate = serviceTickets
        .Where(ticket => ticket.DateCompleted.HasValue)
        .OrderBy(ticket => ticket.DateCompleted)
        .ToList();

    return Results.Ok(ticketsByDate.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        DateCompleted = t.DateCompleted
    }));
});

app.MapGet("tickets/priorities", () =>
{
    var priorities = serviceTickets
        .OrderByDescending(ticket => ticket.Emergency)
        .ThenBy(ticket => ticket.EmployeeId.HasValue ? 1 : 0)
        .ThenBy(ticket => ticket.DateCompleted)
        .ToList();

    return Results.Ok(priorities.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        DateCompleted = t.DateCompleted,
        Emergency = t.Emergency
    }));
});

app.Run();


// {
//     int maxcount = 0;
//     int element_having_max_freq = 0;
//     for (int i = 0; i < n; i++)
//     {
//         int count = 0;
//         for (int j = 0; j < n; j++)
//         {
//             if (arr[i] == arr[j])
//             {
//                 count++;
//             }
//         }

//         if (count > maxcount)
//         {
//             maxcount = count;
//             element_having_max_freq = arr[i];
//         }
//     }

//     return element_having_max_freq;
// }

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();
// app.MapGet("/hello", () =>
// {
//     return "hello";
// });
//

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

