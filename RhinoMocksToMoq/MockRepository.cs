﻿using System;
using System.Collections.Concurrent;
using Moq;

namespace Rhino.Mocks
{
    public static class MockRepository
    {
        private static readonly ConcurrentDictionary<object, Mock> MockStore = new ConcurrentDictionary<object, Mock>();

        public static Mock<T> Get<T>(T obj) where T : class
        {
            if (MockStore.TryGetValue(obj, out var mock))
            {
                return (Mock<T>) mock;
            }
            throw new ArgumentOutOfRangeException(nameof(obj), "Could not find object from mock collection");
        }

        public static T GenerateStub<T>() where T : class
        {
            return GenerateMock<T>();
        }

        public static T GenerateStrictMock<T>() where T : class
        {
            return GenerateMock<T>(MockBehavior.Strict);
        }

        public static T GenerateMock<T>(params object[] args) where T : class
        {
            var mock = new Mock<T>(args);
            if (MockStore.TryAdd(mock.Object, mock))
            {
                return mock.Object;
            }
            throw new Exception("Failed to GenerateMock");
        }

        public static T GenerateMock<T>(MockBehavior behavior = MockBehavior.Default) where T : class
        {
            var mock = new Mock<T>(behavior);
            if (MockStore.TryAdd(mock.Object, mock))
            {
                return mock.Object;
            }
            throw new Exception("Failed to GenerateMock");
        }
    }
}