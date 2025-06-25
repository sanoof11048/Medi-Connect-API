using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medi_Connect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCorrectNursePatientFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NursePayments_NurseAssignments_NurseAssignmentId",
                table: "NursePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NurseAssignments",
                table: "NurseAssignments");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "NurseAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_NurseAssignments",
                table: "NurseAssignments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NurseAssignments_PatientId",
                table: "NurseAssignments",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NursePayments_NurseAssignments_NurseAssignmentId",
                table: "NursePayments",
                column: "NurseAssignmentId",
                principalTable: "NurseAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NursePayments_NurseAssignments_NurseAssignmentId",
                table: "NursePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NurseAssignments",
                table: "NurseAssignments");

            migrationBuilder.DropIndex(
                name: "IX_NurseAssignments_PatientId",
                table: "NurseAssignments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "NurseAssignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NurseAssignments",
                table: "NurseAssignments",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_NursePayments_NurseAssignments_NurseAssignmentId",
                table: "NursePayments",
                column: "NurseAssignmentId",
                principalTable: "NurseAssignments",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
