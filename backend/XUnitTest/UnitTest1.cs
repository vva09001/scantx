using Xunit;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spec_Project.Controllers;
using Spec_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Spec_Project.Models;
using FluentAssertions;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace TestProject
{
    //[TestClass]
    public class UnitTest1
    {
        [Fact]
        public void TestgetCompany()
        {
            var mock = new Mock<ICompanyService>();
            mock.Setup(p => p.getCompany()).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            CompanyController home = new CompanyController(mock.Object);
            var result = (OkObjectResult)home.getCompany();
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }


        [Fact]
        public void TestAddCompany()
        {
            var newadd = GetTest();
            // Arrange
            var mockRepo = new Mock<ICompanyService>();
            mockRepo.Setup(p => p.addCompany(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "ds"

            });
            var controller = new CompanyController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.addCompany(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("ds", actualResponse.Data);
            mockRepo.Verify();
        }

        private CustomerModel GetTest()
        {
            var x = new CustomerModel()
            {
                Cid = "s1",
                Name = "John",
                Address = "dsad",
            };
            return x;
        }

        [Fact]
        public void TestDeleteCompany()
        {
            List<string> x = new List<string>() { "5", "6", "7" };
            var mock = new Mock<ICompanyService>();
            mock.Setup(p => p.DeleteArrCompany(x)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            CompanyController home = new CompanyController(mock.Object);
            var result = (OkObjectResult)home.DeleteArrCompany(x);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }

        [Fact]
        public void TestEditCompany()
        {
            var newadd = GetTest();
            // Arrange
            var mockRepo = new Mock<ICompanyService>();
            mockRepo.Setup(p => p.EditCompany(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"

            });
            var controller = new CompanyController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.EditCompany(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
            mockRepo.Verify();
        }

        // Test ScanData

        private ScanDataModel GetAddScandata()
        {
            var x = new ScanDataModel()
            {
                Uid = 3,
                ScanId = "John",
                CreatedOn = "dsad",
                Payload = "vcn",
                DataType = "string",
                FileName = "ads",
                Status = 1
            };
            return x;
        }

        [Fact]
        public void TestgetScanData()
        {
            var mock = new Mock<IScanDataService>();
            mock.Setup(p => p.getScanData()).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            ScanDataController home = new ScanDataController(mock.Object);
            var result = (OkObjectResult)home.getScanData();
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }



        [Fact]
        public void TestaddScanData()
        {
            var newadd = GetAddScandata();
            // Arrange
            var mockRepo = new Mock<IScanDataService>();
            mockRepo.Setup(p => p.addScanData(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"

            });
            var controller = new ScanDataController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.addScanData(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
            mockRepo.Verify();
        }



        [Fact]
        public void TestDeleteScanData()
        {
            List<string> x = new List<string>() { "5", "6", "7" };
            var mock = new Mock<IScanDataService>();
            mock.Setup(p => p.DeleteArrScanData(x)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            ScanDataController home = new ScanDataController(mock.Object);
            var result = (OkObjectResult)home.DeleteArrScanData(x);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }

        [Fact]
        public void TestEditScandata()
        {
            var newadd = GetAddScandata();
            // Arrange
            var mockRepo = new Mock<IScanDataService>();
            mockRepo.Setup(p => p.EditScandata(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"

            });
            var controller = new ScanDataController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.EditScandata(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
            mockRepo.Verify();
        }

        [Fact]
        public void TestCheckUID()
        {
            var mock = new Mock<IScanDataService>();

            mock.Setup(p => p.CheckUserScanData("5")).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            ScanDataController home = new ScanDataController(mock.Object);
            var result = (OkObjectResult)home.CheckUserScanData("5");
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }

        // Test user
        private UserDto GetUser()
        {
            var tbluser = new UserDto
            {

                UserName = "abc",
                FamilyName = "acb",
                GivenName = "ascc",
                TypeOfAccount = "ascascasc",
                Email = "acasc",
                ContactByEmail = true,
                EncryptionActive = true,
                Cid = "asdasdasd",
                RoleID = 4
            };
            return tbluser;
        }

        [Fact]
        public void TestgetUser()
        {
            var mock = new Mock<IUserService>();
            var mock1 = new Mock<IMapper>();
            var mock2 = new Mock<IOptions<Spec_Project.Helpers.AppSettings>>();
            mock.Setup(p => p.getUser()).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            UsersController home = new UsersController(mock.Object, mock1.Object, mock2.Object);
            var result = (OkObjectResult)home.GetUser();
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }

        [Fact]
        public void TestDeleteUser()
        {
            List<int> x = new List<int>() { 5, 6, 7 };
            var mock = new Mock<IUserService>();
            var mock1 = new Mock<IMapper>();
            var mock2 = new Mock<IOptions<Spec_Project.Helpers.AppSettings>>();
            mock.Setup(p => p.DeleteArrUser(x)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            UsersController home = new UsersController(mock.Object, mock1.Object, mock2.Object);
            var result = (OkObjectResult)home.DeleteArrUser(x);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }

        [Fact]
        public void TestAddUser()
        {
            var tbluser = new UserDto
            {

                UserName = "abc",
                FamilyName = "acb",
                GivenName = "ascc",
                TypeOfAccount = "ascascasc",
                Email = "acasc",
                ContactByEmail = true,
                EncryptionActive = true,
                Cid = "asdasdasd",
                RoleID = 4
            };
            var password = "123";
            var UserIDLogin = "saa";
            // Arrange
            var mock = new Mock<IUserService>();
            var mock1 = new Mock<IMapper>();
            var mock2 = new Mock<IOptions<Spec_Project.Helpers.AppSettings>>();
            mock.Setup(p => p.AddUser(tbluser, password, UserIDLogin)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = null

            });
            var controller = new UsersController(mock.Object, mock1.Object, mock2.Object);

            // Act
            var result = controller.AddUser(tbluser, password, UserIDLogin);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result);
            Assert.Equal(null, actualResponse.Data);
            mock.Verify();
        }

        [Fact]
        public void TestEditUser()
        {
            var tbluser = new UsersModel
            {

                Id = 4,
                Cid = "userDto.Cid",
                ContactByEmail = true,
                Email = "userDto.Email",
                EncryptionActive = true,
                FamilyName = "userDto.FamilyName",
                GivenName = "userDto.GivenName",
                TypeOfAccount = "userDto.TypeOfAccount",
                UserName = "userDto.UserName",
                PasswordHash = { },
                PasswordSalt = { },
                RoleID = 2


            };
            var password = "123";
            var UserIDLogin = "saa";
            // Arrange
            var mock = new Mock<IUserService>();
            var mock1 = new Mock<IMapper>();
            var mock2 = new Mock<IOptions<Spec_Project.Helpers.AppSettings>>();
            mock.Setup(p => p.Update(tbluser, password)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = null

            });
            var controller = new UsersController(mock.Object, mock1.Object, mock2.Object);

            // Act
            var result = controller.Update(tbluser);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result);
            result.Data.Should().BeEquivalentTo(actualResponse.Data);

            mock.Verify();
        }
    }
}