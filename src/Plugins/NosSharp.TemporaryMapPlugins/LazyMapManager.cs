﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;

namespace NosSharp.TemporaryMapPlugins
{
    public class LazyMapManager : IMapManager
    {
        private readonly Dictionary<Guid, IMapLayer> _mapLayers = new Dictionary<Guid, IMapLayer>();
        private readonly Dictionary<long, IMap> _maps = new Dictionary<long, IMap>();

        private IMap LoadMap(long mapId)
        {
            MapDto map = ChickenContainer.Instance.Resolve<IMapService>().GetById(mapId);
            IEnumerable<MapNpcDto> npcs = ChickenContainer.Instance.Resolve<IMapNpcService>().GetByMapId(mapId);
            IEnumerable<MapMonsterDto> monsters = ChickenContainer.Instance.Resolve<IMapMonsterService>().GetByMapId(mapId);
            IEnumerable<PortalDto> portals = ChickenContainer.Instance.Resolve<IPortalService>().GetByMapId(mapId);
            IEnumerable<ShopDto> shops = ChickenContainer.Instance.Resolve<IShopService>().GetByMapNpcIds(npcs.Select(s => s.Id).ToList());
            return new SimpleMap(map, monsters, npcs, portals, shops);
        }

        public IReadOnlyDictionary<long, IMap> Maps { get; }
        public void ChangeMap(IPlayerEntity player, long mapId)
        {
            throw new NotImplementedException();
        }

        public void ChangeMapLayer(IPlayerEntity player, Guid mapLayerId)
        {
            throw new NotImplementedException();
        }

        public void ChangeMapLayer(IPlayerEntity player, IMapLayer layer)
        {
            throw new NotImplementedException();
        }

        public IMapLayer GetBaseMapLayer(long mapId)
        {
            if (_maps.TryGetValue(mapId, out IMap map))
            {
                return map.BaseLayer;
            }

            map = LoadMap(mapId);
            _maps.Add(mapId, map);
            _mapLayers.Add(map.BaseLayer.Id, map.BaseLayer);

            return map.BaseLayer;
        }

        public IMapLayer GetBaseMapLayer(IMap map)
        {
            return map.BaseLayer;
        }
    }
}