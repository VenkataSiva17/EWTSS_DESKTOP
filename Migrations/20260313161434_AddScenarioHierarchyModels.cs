using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EWTSS_DESKTOP.Migrations
{
    /// <inheritdoc />
    public partial class AddScenarioHierarchyModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Features_FeatureId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenarios_Users_UserId",
                table: "Scenarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Scenarios",
                table: "Scenarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Features",
                table: "Features");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Scenarios",
                newName: "scenario");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "role");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "role_permission");

            migrationBuilder.RenameTable(
                name: "Features",
                newName: "feature");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "user",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "user",
                newName: "updated_on");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "user",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "user",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "user",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "user",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "user",
                newName: "created_on");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "user",
                newName: "created_by");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "user",
                newName: "IX_user_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "scenario",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "scenario",
                newName: "updated_on");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "scenario",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "StartStop",
                table: "scenario",
                newName: "start_stop");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "scenario",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "ScenarioType",
                table: "scenario",
                newName: "scenario_type");

            migrationBuilder.RenameColumn(
                name: "ScenarioStatus",
                table: "scenario",
                newName: "scenario_status");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "scenario",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "ExecuteTime",
                table: "scenario",
                newName: "execute_time");

            migrationBuilder.RenameColumn(
                name: "ExecuteRun",
                table: "scenario",
                newName: "execute_run");

            migrationBuilder.RenameColumn(
                name: "ExecuteDate",
                table: "scenario",
                newName: "execute_date");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "scenario",
                newName: "created_on");

            migrationBuilder.RenameIndex(
                name: "IX_Scenarios_UserId",
                table: "scenario",
                newName: "IX_scenario_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "role_permission",
                newName: "IX_role_permission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_FeatureId",
                table: "role_permission",
                newName: "IX_role_permission_FeatureId");

            migrationBuilder.AlterColumn<string>(
                name: "start_stop",
                table: "scenario",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "scenario_type",
                table: "scenario",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "scenario_status",
                table: "scenario",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "execute_run",
                table: "scenario",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_scenario",
                table: "scenario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role",
                table: "role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_feature",
                table: "feature",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "area_operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Altitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    scenario_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area_operation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_area_operation_scenario_scenario_id",
                        column: x => x.scenario_id,
                        principalTable: "scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "area_operation_polygon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Altitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_operation_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area_operation_polygon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_area_operation_polygon_area_operation_area_operation_id",
                        column: x => x.area_operation_id,
                        principalTable: "area_operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_operation_id = table.Column<int>(type: "int", nullable: false),
                    line_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_line", x => x.Id);
                    table.ForeignKey(
                        name: "FK_line_area_operation_area_operation_id",
                        column: x => x.area_operation_id,
                        principalTable: "area_operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cc_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    line_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cc_line_line_id",
                        column: x => x.line_id,
                        principalTable: "line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "emitter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    platform_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mode_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    power_transmitted = table.Column<double>(type: "double", nullable: true),
                    start_frequency_value = table.Column<double>(type: "double", nullable: true),
                    stop_frequency_value = table.Column<double>(type: "double", nullable: true),
                    hop_period_value = table.Column<double>(type: "double", nullable: true),
                    hop_period_unit = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hop_inter_period_value = table.Column<double>(type: "double", nullable: true),
                    hop_inter_period_unit = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bandwidth = table.Column<double>(type: "double", nullable: true),
                    modulation_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pattern_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    scan_rate = table.Column<double>(type: "double", nullable: true),
                    antenna_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gain = table.Column<double>(type: "double", nullable: true),
                    Polarization = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    line_id = table.Column<int>(type: "int", nullable: false),
                    emitter_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emitter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_emitter_line_line_id",
                        column: x => x.line_id,
                        principalTable: "line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "entity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_frequency_value = table.Column<double>(type: "double", nullable: true),
                    stop_frequency_value = table.Column<double>(type: "double", nullable: true),
                    antenna_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Polarization = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    antenna_height = table.Column<int>(type: "int", nullable: true),
                    scan_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cc_id = table.Column<int>(type: "int", nullable: false),
                    entity_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entity_cc_cc_id",
                        column: x => x.cc_id,
                        principalTable: "cc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "entity_polygon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    entity_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_polygon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entity_polygon_entity_entity_id",
                        column: x => x.entity_id,
                        principalTable: "entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_area_operation_scenario_id",
                table: "area_operation",
                column: "scenario_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_operation_polygon_area_operation_id",
                table: "area_operation_polygon",
                column: "area_operation_id");

            migrationBuilder.CreateIndex(
                name: "IX_cc_line_id",
                table: "cc",
                column: "line_id");

            migrationBuilder.CreateIndex(
                name: "IX_emitter_line_id",
                table: "emitter",
                column: "line_id");

            migrationBuilder.CreateIndex(
                name: "IX_entity_cc_id",
                table: "entity",
                column: "cc_id");

            migrationBuilder.CreateIndex(
                name: "IX_entity_polygon_entity_id",
                table: "entity_polygon",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_line_area_operation_id",
                table: "line",
                column: "area_operation_id");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_feature_FeatureId",
                table: "role_permission",
                column: "FeatureId",
                principalTable: "feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_role_RoleId",
                table: "role_permission",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_scenario_user_user_id",
                table: "scenario",
                column: "user_id",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_role_id",
                table: "user",
                column: "role_id",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_feature_FeatureId",
                table: "role_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_role_RoleId",
                table: "role_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_scenario_user_user_id",
                table: "scenario");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_role_id",
                table: "user");

            migrationBuilder.DropTable(
                name: "area_operation_polygon");

            migrationBuilder.DropTable(
                name: "emitter");

            migrationBuilder.DropTable(
                name: "entity_polygon");

            migrationBuilder.DropTable(
                name: "entity");

            migrationBuilder.DropTable(
                name: "cc");

            migrationBuilder.DropTable(
                name: "line");

            migrationBuilder.DropTable(
                name: "area_operation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_scenario",
                table: "scenario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_feature",
                table: "feature");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "scenario",
                newName: "Scenarios");

            migrationBuilder.RenameTable(
                name: "role_permission",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "feature",
                newName: "Features");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "updated_on",
                table: "Users",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "created_on",
                table: "Users",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Users",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_id",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Scenarios",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_on",
                table: "Scenarios",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "Scenarios",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "start_stop",
                table: "Scenarios",
                newName: "StartStop");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Scenarios",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "scenario_type",
                table: "Scenarios",
                newName: "ScenarioType");

            migrationBuilder.RenameColumn(
                name: "scenario_status",
                table: "Scenarios",
                newName: "ScenarioStatus");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Scenarios",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "execute_time",
                table: "Scenarios",
                newName: "ExecuteTime");

            migrationBuilder.RenameColumn(
                name: "execute_run",
                table: "Scenarios",
                newName: "ExecuteRun");

            migrationBuilder.RenameColumn(
                name: "execute_date",
                table: "Scenarios",
                newName: "ExecuteDate");

            migrationBuilder.RenameColumn(
                name: "created_on",
                table: "Scenarios",
                newName: "CreatedOn");

            migrationBuilder.RenameIndex(
                name: "IX_scenario_user_id",
                table: "Scenarios",
                newName: "IX_Scenarios_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_RoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_FeatureId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_FeatureId");

            migrationBuilder.AlterColumn<int>(
                name: "StartStop",
                table: "Scenarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ScenarioType",
                table: "Scenarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ScenarioStatus",
                table: "Scenarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ExecuteRun",
                table: "Scenarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scenarios",
                table: "Scenarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Features",
                table: "Features",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Features_FeatureId",
                table: "RolePermissions",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenarios_Users_UserId",
                table: "Scenarios",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
