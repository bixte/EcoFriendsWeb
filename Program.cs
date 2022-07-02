using EcoFriendsWeb.DataModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoFriendsWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var dataBase = services.GetRequiredService<AppDataBaseContext>();

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!dataBase.BlogPosts.Any())
            {
                await dataBase.BlogPosts.AddAsync(new BlogPost()
                {
                    Title = "7 �������, ��� ����� �����������",
                    ShortDesctiption = "�� ������� ���� ��������, ������� ��������� � ���, ��� ����� ��������, � ����� �� ��� �������� ������",
                    Desctiption = "������������� ������������, ������� ������ �������� ���������������� ��� ������������������, ���� ����������������� �������, � ������ �������� ����� ������ �� ���������� �����. � ���������������� ����� ����� ���� ��������� ����������, ����� ������� ���������� ������, ����� �� ��������� ��������� �������������, ����� � ����� zero waste (����� ���������� ������� �������� � ���������� ��������), ����� �� �������, ������������� �������������� ���������� ������� � ������ ������. ������������������ � ��� ��� ��������. ��� ����� � �������� ���� ����� ���������� ��� ���� ������, ����� ������� �� ���������� ������ ���� �������, ��������� � ��������� ������������ �� ���������� �����, � ������� ������� ����������, �������������� ������� ������� ��� ������.",
                    TimeCreate = DateTime.Now.ToString("d"),
                    Tags = new List<Tag>() { new Tag("TOP �������") },
                    MainImagesStore = new List<ImagePicture>() { new ImagePicture("eco1", "/img/blog/Eco0.png"), new ImagePicture("eco2", "/img/blog/Eco2.jpeg"), new ImagePicture("eco3", "/img/blog/naturemyluv.jpg") },
                    PostParticals = new List<PostPartical>()
                    {
                        new PostPartical()
                        {
                            Title = "�������� ���� ��",
                            ImagesStore = new List<ImagePicture>() { new ImagePicture("naturemyluv", "/img/blog/Eco1.jpeg") },
                            SubTexts = new List<SubText>
                            {
                                new SubText()
                                {
                                    Title = "��� ����� ������������� ������������?",
                                    Text = "������������� ������������, ������� ������ �������� ���������������� ��� ������������������, ���� ����������������� �������, � ������ �������� ����� ������ �� ���������� �����. � ���������������� ����� ����� ���� ��������� ����������, ����� ������� ���������� ������, ����� �� ��������� ��������� �������������, ����� � ����� zero waste (����� ���������� ������� �������� � ���������� ��������), ����� �� �������, ������������� �������������� ���������� ������� � ������ ������. ������������������ � ��� ��� ��������. ��� ����� � �������� ���� ����� ���������� ��� ���� ������, ����� ������� �� ���������� ������ ���� �������, ��������� � ��������� ������������ �� ���������� �����. � ��� �� ����� �� ������� ��������� ������������� ������������ �������� ������������� ��������� � ��������. � ��������� �������� � ����������� ����������� ��������� �� ������ �������� � �������� ������� ����� ��������� ��������� ��� ���������, �� � ������������ ��������� � ���������, ���������� � �����������. ������� ������� ������������ �� ���, ��� �������� ����� ����� �������, ������� ����������, ����� �������� �� ��������� � �� ������������ �� �������."
                                },
                                new SubText()
                                {
                                    Title = "��� ����� ������������� ���������?",
                                    Text ="��� ��� �������� ��������: ����� ������. ����� ������� ����������� ������� �� ��������� ������� � ������ �����, ����� �������� ���������� � ����, ��� ������� ����� ���������� � ��������� ������. ���, ����� ��������� ����������� ��������� ��������� ������������� ��� ������ �������� ������� ������ � ����� � ��������� � ������� ����. � ���� � ����������� ������ ���� ������� ������� �������, ����� ����������� ��������� ���������� ������������ ��������, � ����� ����� ������ ������ ��������� � ����� ������. ��� ����� ������� ������ ��������������� ����� �������� �������� �� �������������� ��������� �������, ��������, ��������� ��� ��������. ����������, ����� ��� ������� ������������ ������� � ������� ���������� ��������. ������������ ��� ����������, �� ����������� ���� ������, ���� �������� � ���� ����� � ����� ����������� ����, ����� ��� ����������� ���������."
                                }                     
                            }
                        },
                        new PostPartical()
                        {
                            ImagesStore = new List<ImagePicture>(){new ImagePicture("Eco","/img/blog/Eco3.jpeg"), new ImagePicture("EcoPrib", "/img/blog/EcoPrib.jpeg") },
                            SubTexts = new List<SubText>
                            {
                                new SubText()
                                {
                                    Title = "��� ������ ����������� �����?",
                                    Text= "��� ������ ������� ���������� �� ��������������. ���� �� ������� ������ �� ����������� �����, �� �� ����� ������� �������� ���������������� ������ � ���������, �������, ������� � ��������. ����� �������� ����� ������ ����������� ��������������, ����� ���� ������� ������� � ������������ ������������� �����-���� ������. � ����������� ������������� ���� ���� �������� ��� ��������, � ������� ����� ������ ���: � ������� ������ ����������� ������ �� �� �������, ��� � ������, � �������������� �������� ��������� � ���� �����/�������/�������. ������ �������, ����� ��� ��������, ������� �� ����������� �����������, ���� ������ � �����. ��� ����� ��� �������� ����� �������� �� ������ � �������� ��� � ������ ����������. ����� ���� �������� ��������� � ����� �� �� ����������� ��������, � ��������� ����������. ����� ����� ����� ���������� �������� � �����, ������� ������� ����� ������� ��������� � ����������� ����, ����� ��� �������� ��������� �����������, � ����� ���. ����� ���������� �������: ����� ����� ������ ���������, ���� �� ������������ ����������� ����� ���� ������, � ��������� ������ ��������, ������� ��������� ������ ���. ����� �������, ������ ���������, ��������� ��������� �� ����� �������� �� �������� ���������: �������, ������, ������, ������ � ������������� ��������, ����� Tetra Pak  � ����� �������� ����������. ����� ����� ����� ���������� � ������������ ���� ������ ����� �������� ������� �������� ����������� ���������� ����� � ���� �� ������� ���������� �� �������� �������� ����������� �����. ������������ ������ � ��� ������� ����, ������� ������ ������������ � �������� �����, � ����� �� ������. � �������� ��� ������� �� ������, �������, ������, ������� ����� � ������. ����� ���������� �� ����� ������� ���������� ����������� ��� ����: ����������� ���������, ������ � �����, ���� ��������� ������ ����� ����������. � ����������� ����������, ������� ��� �������� ���������� ������, ������� ���� ����� ��������� �� ������ �� ������. ����� ������ �� ������������ ���������� ����� � �� ������������ � ������ �����, �� �������� ������ ������������ ��������� ���� �� �����. ��� ��������, ������, ������������ ����, ��� ������� ������� ������������ � ������� �������. ",
                                },
                                new SubText()
                                {
                                    Title = "��� ����� ������������� ������?",
                                    Text = "� ������������� ������� ��������� ��������, ��������� � ���������� ���� ������������. ��� �� ���������� ����� ����������� ��������, ������� ������ ������ ���������, ����� ������� ��� ������, �� �� ���������. � ������� ���� ��������� ������� �� ���������� ��������, �� ����������� ����� �� ����������. ����� �� ������� �������������� ��������� � ������� ������ ������, �� ����������� ���������, ���� �� ����� ��������, � ������������� ����������. � ���������, �� ���� ������������ � ��������� ����� ������. ���������� ���� ��������, �� ����� ���������� �� ����������� � ������, � ��������� ������� ����������. �� ���� ���� �����, ������� ������� ����������� ������������� ��������� ���� ����� ��������: ������ �������������� �������� ����������� ������������ � ��� ���� � ������, �� �������� ������. � ���� �� � ������� �� ����� ������ �� ���������� �����������, ������� ��������� ������� ������� � ������������� �������. ��� �������� �������������� ��������, �� � ��� ���������� ������� ����, ��� ����� ����� ��� ���������� ����� � ������ ��������� ����������.",
                                }
                            }
                        }
                    }
                });
                await dataBase.SaveChangesAsync();
            }
            if (!dataBase.EventPosts.Any())
            {
                await dataBase.EventPosts.AddAsync(new EventPost()
                {
                    Title = "Open Space Market",
                    ShortDescription = "��� ����� ��� ������ �������� ������������ ����!",
                    Tags = new List<Tag>() { new Tag("Kazan"), new Tag("#openspacemarket") },
                    Desctiption = "� �� ����� ������������ ������ Open Space Market ������ 25 � 26 ���� �� ���������� �� ���������� ������� (������������ �����, 8) � 11:00 �� 20:00. �� ���� �������� ���� ������ ��� ����������!",
                    TimeCreate = DateTime.Now.ToString("d"),
                    ImagesStore = new List<ImagePicture>() { new ImagePicture("openSpace", "/img/events/openSpace2.jpg"), new ImagePicture("openSpace2", "/img/events/openSpace.jpg"), new ImagePicture("openSpace3", "/img/events/openSpace3.jpg") },
                    ContactAdress = "�� ���������� �������",
                    ContactEmail = "kazan@openspacemarket.ru",
                    ContactPhone = "89503285152",
                    Location = "�� ���������� �������",
                    Text = "���� ��������� ������ ������ ���, ��� �� ���� ���������� ��������.����� ��������� ����� � ������ ����������, � ����� ������ ����� �� �������� � ����� ������.",
                    Text2 = "�� ����������� �������� ������ � ������������� ��� ������������� �������. ���� �� �������� ���� ������, ����������� �������� �� ���� ������. ������� ������� � ��������� �� ���������� � ����� �� ������.",
                    EventData = "25 � 26 ���� � 11:00 �� 20:00",
                });
                await dataBase.SaveChangesAsync();
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}