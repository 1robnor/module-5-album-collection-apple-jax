﻿using album_collection.Models;
using album_collection.Repositories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using album_collection.Controllers;
using System.Linq;

namespace album_collection.Tests
{
    public class AlbumControllerTest
    {
        AlbumsController underTest;
        IRepository<Album> albumRepo;
        public AlbumControllerTest()
        {
            albumRepo = Substitute.For<IRepository<Album>>();
            underTest = new AlbumsController(albumRepo);

        }

        [Fact]
        public void Get_Returns_3_Albums()
        {
            //Arrange
            var myCollection = new List<Album>()
            {
                new Album(1, "First Album", "columbia records", "img", 1),
                new Album(2, "Second Album", "columbia records", "img", 1),
                new Album(3, "Third Album", "columbia records", "img", 1)

            };
            underTest.GetAlbum().Returns(myCollection);

            //Act
            var result = underTest.GetAlbum();
            var countOfAlbums = result.Count();

            //Assert
            Assert.Equal(3, countOfAlbums);
        }

        // Alternative test for Get() action
        [Fact]
        public void Get_Returns_List_of_Albums()
        {
            // arrange
            var expectedAlbums = new List<Album>()
            {
                new Album(1, "First Album", "columbia records", "img", 1),
                new Album(2, "Second Album", "columbia records", "img", 1),
                new Album(3, "Third Album", "columbia records", "img", 1)

            };

            underTest.GetAlbum().Returns(expectedAlbums);

            // act
            var result = underTest.GetAlbum();

            // assert
            Assert.Equal(expectedAlbums, result.ToList());
        }

        [Fact]
        public void Get_by_id_Returns_Chosen_Album()
        {
            // arrange
            var id = 2;
            var firstAlbum = new Album(1, "First Album", "columbia records", "img", 1);
            var secondAlbum = new Album(2, "Second Album", "columbia records", "img", 1);
            var expectedAlbums = new List<Album>();
            expectedAlbums.Add(firstAlbum);
            expectedAlbums.Add(secondAlbum);

            // We need to mock the Repository's GetById() method
            albumRepo.GetById(id).Returns(secondAlbum);

            // act
            var result = underTest.GetAlbum(id);

            // assert
            Assert.Equal(secondAlbum, result);
        }

        [Fact]
        public void Post_Creates_New_Album()
        {
            // arrange
            var newAlbum = new Album(1, "First Album", "columbia records", "img", 1);
            var albumList = new List<Album>();

            // Use When..Do to substitute for methods that don't return a value, like the Repository method Create()
            // When() allows us to call the method on the substitute and pass an argument
            // Do() allows us to pass a callback function that executes when the method is called
            albumRepo.When(t => t.Create(newAlbum))
                .Do(t => albumList.Add(newAlbum));

            albumRepo.GetAll().Returns(albumList);

            // act
            var result = underTest.PostAlbum(newAlbum);

            // assert
            Assert.Contains(newAlbum, result);
        }




    }
}
