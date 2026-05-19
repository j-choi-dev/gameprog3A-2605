using SampleAppSystemSDK.Application;
using SampleAppSystemSDK.Domain;
using SampleAppSystemSDK.Infrastructure;
using System;
using UnityEngine;
using Zenject;

namespace SampleAppSystemSDK.Installer
{
    public class SampleAppSystemSDKInstaller : MonoInstaller
    {
        [SerializeField] private KeyInputInfrastructure _keyInput = null;
        public override void InstallBindings()
        {
            // Application
            Container
                .Bind<IFileSystemApllication>()
                    .To<FileSystemApllication>()
                    .AsSingle();
            Container
                .Bind<IParseApplication>()
                    .To<ParseApplication>()
                    .AsSingle();
            Container
                .Bind<IKeyInputApplication>()
                    .To<KeyInputApplication>()
                    .AsSingle()
                    .NonLazy();

            // Domain
            Container
                .Bind<IFileSystemDomain>()
                    .To<FileSystemInfrastructure>()
                    .AsSingle();
            Container
                .Bind<IParseDomain>()
                    .To<CSVParseInfrastructure>()
                    .AsSingle();
            Container
                .Bind<IKeyInput>()
                    .FromInstance(_keyInput);
        }
    }
}
