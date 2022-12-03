﻿// Copyright (C) 2022 Nicholas Maltbie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using Unity.Netcode.TestHelpers.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace nickmaltbie.openkcc.Tests.netcode.TestCommon
{
    public abstract class NetcodeInputRuntimeTest
    {
        protected abstract int NumberOfClients { get; }
        protected NetcodeIntegrationTestHelper netcodeHelper;
        protected InputTestFixture input;

        public static implicit operator NetcodeIntegrationTest(NetcodeInputRuntimeTest nt)
        {
            return nt.netcodeHelper;
        }

        public sealed class NetcodeIntegrationTestHelper : NetcodeIntegrationTest
        {
            private NetcodeInputRuntimeTest netcodeInputRuntime;

            public NetcodeIntegrationTestHelper(NetcodeInputRuntimeTest netcodeInputRuntime)
            {
                this.netcodeInputRuntime = netcodeInputRuntime;
            }

            public new GameObject m_PlayerPrefab => base.m_PlayerPrefab;
            public new NetworkManager m_ServerNetworkManager => base.m_ServerNetworkManager;
            public new NetworkManager[] m_ClientNetworkManagers => base.m_ClientNetworkManagers;

            protected override int NumberOfClients => netcodeInputRuntime.NumberOfClients;

            protected override void OnServerAndClientsCreated()
            {
                netcodeInputRuntime.OnServerAndClientsCreated();
            }

            public new GameObject CreateNetworkObjectPrefab(string baseName)
            {
                return base.CreateNetworkObjectPrefab(baseName);
            }

            public new GameObject SpawnObject(GameObject prefabGameObject, NetworkManager owner, bool destroyWithScene = false)
            {
                return base.SpawnObject(prefabGameObject, owner, destroyWithScene);
            }

            public new List<GameObject> SpawnObjects(GameObject prefabGameObject, NetworkManager owner, int count, bool destroyWithScene = false)
            {
                return base.SpawnObjects(prefabGameObject, owner, count, destroyWithScene);
            }
        }

        public NetcodeInputRuntimeTest()
        {
            netcodeHelper = new NetcodeIntegrationTestHelper(this);
            input = new InputTestFixture();
        }

        /// <summary>
        /// This is invoked before the server and client(s) are started.
        /// Override this method if you want to make any adjustments to their
        /// NetworkManager instances.
        /// </summary>
        protected virtual void OnServerAndClientsCreated()
        {

        }

        /// <summary>
        /// Creates a basic NetworkObject test prefab, assigns it to a new
        /// NetworkPrefab entry, and then adds it to the server and client(s)
        /// NetworkManagers' NetworkConfig.NetworkPrefab lists.
        /// </summary>
        /// <param name="baseName">the basic name to be used for each instance</param>
        /// <returns>NetworkObject of the GameObject assigned to the new NetworkPrefab entry</returns>
        protected GameObject CreateNetworkObjectPrefab(string baseName)
        {
            return netcodeHelper.CreateNetworkObjectPrefab(baseName);
        }

        protected GameObject SpawnObject(GameObject prefabGameObject, NetworkManager owner, bool destroyWithScene = false)
        {
            return netcodeHelper.SpawnObject(prefabGameObject, owner, destroyWithScene);
        }

        protected List<GameObject> SpawnObjects(GameObject prefabGameObject, NetworkManager owner, int count, bool destroyWithScene = false)
        {
            return netcodeHelper.SpawnObjects(prefabGameObject, owner, count, destroyWithScene);
        }

        protected GameObject PlayerPrefab => netcodeHelper.m_PlayerPrefab;
        protected NetworkManager ServerNetworkManager => netcodeHelper.m_ServerNetworkManager;
        protected NetworkManager[] ClientNetworkManagers => netcodeHelper.m_ClientNetworkManagers;

        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);
            }
#endif
            netcodeHelper.OneTimeSetup();
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            netcodeHelper.OneTimeTearDown();
        }

        [SetUp]
        public virtual void SetUp()
        {
            input.Setup();
        }

        [UnitySetUp]
        public virtual IEnumerator UnitySetUp()
        {
            return netcodeHelper.SetUp();
        }

        [TearDown]
        public virtual void TearDown()
        {
            input.TearDown();
        }

        [UnityTearDown]
        public virtual IEnumerator UnityTearDown()
        {
            return netcodeHelper.TearDown();
        }
    }
}
