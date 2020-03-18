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
    public class SongControllerTest
    {
        public class SongControllerTests
        {
            SongsController underTest;
            IRepository<Song> songRepo;

            public SongControllerTests()
            {
                songRepo = Substitute.For<IRepository<Song>>();
                underTest = new SongsController(songRepo);

            }

            [Fact]
            public void Get_Returns_3_Songs()
            {
                //Arrange
                var myCollection = new List<Song>()
            {
                new Song(1,"First Song","3:12","song.com", 1),
                new Song(2,"Second Song","3:12","song.com", 1),
                new Song(3,"Third Song","3:12","song.com", 1)
            };

                songRepo.GetAll().Returns(myCollection);

                //Act
                var result = underTest.GetSongs();
                var countOfSongs = result.Count();

                //Assert
                Assert.Equal(3, countOfSongs);
            }

            // Alternative test for Get() action
            [Fact]
            public void Get_Returns_List_of_Songs()
            {
                // arrange
                var expectedSongs = new List<Song>()
            {
                new Song(1,"First Song","3:12","song.com", 1),
                new Song(2,"Second Song","3:12","song.com", 1),
                new Song(3,"Third Song","3:12","song.com", 1)
            };

                songRepo.GetAll().Returns(expectedSongs);

                // act
                var result = underTest.GetSongs();

                // assert
                Assert.Equal(expectedSongs, result.ToList());
            }

            [Fact]
            public void Get_by_id_Returns_Chosen_Song()
            {
                // arrange
                var id = 2;
                var firstSong = new Song(1, "First Song", "3:12", "song.com", 1);
                var secondSong = new Song(2, "Second Song", "3:12", "song.com", 1);
                var expectedSongs = new List<Song>();
                expectedSongs.Add(firstSong);
                expectedSongs.Add(secondSong);

                // We need to mock the Repository's GetById() method
                songRepo.GetById(id).Returns(secondSong);

                // act
                var result = underTest.GetSongs(id);

                // assert
                Assert.Equal(secondSong, result);
            }

            [Fact]
            public void Post_Creates_New_Song()
            {
                // arrange
                var newSong = new Song(1, "First Song", "3:12", "song.com", 1);
                var songList = new List<Song>();

                // Use When..Do to substitute for methods that don't return a value, like the Repository method Create()
                // When() allows us to call the method on the substitute and pass an argument
                // Do() allows us to pass a callback function that executes when the method is called
                songRepo.When(t => t.Create(newSong))
                    .Do(t => songList.Add(newSong));

                songRepo.GetAll().Returns(songList);

                // act
                var result = underTest.PostSong(newSong);

                // assert
                Assert.Contains(newSong, result);
            }

        }
           
    }
}
