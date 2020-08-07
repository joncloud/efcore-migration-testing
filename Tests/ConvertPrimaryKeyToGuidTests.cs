using Source;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ConvertPrimaryKeyToGuidTests
    {
        [Fact]
        public async Task Down_ShouldConvertIdToInt_GivenValueLessThanInt32Max()
        {
            var num = 1234;
            var hex = 0x1234;
            var id = new Guid($"{num:00000000}-0000-0000-0000-000000000000");
            var expected = "ABC";
            await Migrate.From<Source.Migrations.ConvertPrimaryKeyToGuid>()
                .Setup(
                    conn => conn.ExecuteNonQueryAsync(cmd =>
                    {
                        cmd.CommandText = @"
                            INSERT INTO dbo.Students (Id, Name)
                            VALUES(@Id, @Name)
                        ";
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Name", expected);
                    })
                )
                .To<Source.Migrations.Initial>()
                .AssertAsync<SchoolContext>(async (_, conn) =>
                {
                    var actual = await conn.ExecuteScalarAsync<string>(cmd =>
                    {
                        cmd.CommandText = @"
                            SELECT s.Name
                            FROM Students s
                            WHERE s.Id = @Id
                        ";
                        cmd.Parameters.AddWithValue("@Id", hex);
                    });

                    Assert.Equal(expected, actual);
                });
        }

        [Fact]
        public async Task Up_ShouldConvertIdToGuid()
        {
            var expected = "ABC";
            await Migrate.From<Source.Migrations.Initial>()
                .Setup(
                    conn => conn.ExecuteScalarAsync<int>(cmd =>
                    {
                        cmd.CommandText = @"
                            INSERT INTO Students (Name)
                            VALUES(@Name);
                            SELECT CAST(@@IDENTITY AS int);
                        ";
                        cmd.Parameters.AddWithValue("@Name", expected);
                    })
                )
                .To<Source.Migrations.ConvertPrimaryKeyToGuid>()
                .AssertAsync<SchoolContext>(async (id, conn) =>
                {
                    var convertedId = new Guid($"{id:00000000}-0000-0000-0000-000000000000");

                    var actual = await conn.ExecuteScalarAsync<string>(cmd =>
                    {
                        cmd.CommandText = @"
                            SELECT s.Name
                            FROM Students s
                            WHERE s.Id = @Id
                        ";
                        cmd.Parameters.AddWithValue("@Id", convertedId);
                    });

                    Assert.Equal(expected, actual);
                });
        }
    }
}
