﻿// <auto-generated />
using System;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(DbContextClass))]
    [Migration("20231114014250_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Models.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("QueueId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("QueueId")
                        .IsUnique();

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Models.ClientTopic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("TopicId");

                    b.ToTable("ClientTopic");
                });

            modelBuilder.Entity("Models.MessageRecevied", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("SendMessageDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("MessageRecevied");
                });

            modelBuilder.Entity("Models.QueueTopic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("QueuesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("QueuesId");

                    b.HasIndex("TopicId");

                    b.ToTable("QueueTopic");
                });

            modelBuilder.Entity("Models.Queues", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Queues");
                });

            modelBuilder.Entity("Models.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RoutingKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("Models.Client", b =>
                {
                    b.HasOne("Models.Queues", "Queue")
                        .WithOne("Client")
                        .HasForeignKey("Models.Client", "QueueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Queue");
                });

            modelBuilder.Entity("Models.ClientTopic", b =>
                {
                    b.HasOne("Models.Client", "Client")
                        .WithMany("ClientTopic")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Topic", "Topic")
                        .WithMany("ClientTopic")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Models.MessageRecevied", b =>
                {
                    b.HasOne("Models.Client", "Client")
                        .WithMany("Messages")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Models.QueueTopic", b =>
                {
                    b.HasOne("Models.Queues", "Queues")
                        .WithMany("QueueTopics")
                        .HasForeignKey("QueuesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Topic", "Topic")
                        .WithMany("QueueTopics")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Queues");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Models.Client", b =>
                {
                    b.Navigation("ClientTopic");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Models.Queues", b =>
                {
                    b.Navigation("Client");

                    b.Navigation("QueueTopics");
                });

            modelBuilder.Entity("Models.Topic", b =>
                {
                    b.Navigation("ClientTopic");

                    b.Navigation("QueueTopics");
                });
#pragma warning restore 612, 618
        }
    }
}
