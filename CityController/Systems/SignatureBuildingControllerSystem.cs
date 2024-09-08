using Colossal.Serialization.Entities;
using CS2Shared.Common;
using Game.Objects;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;

namespace CityController.Systems;

public partial class SignatureBuildingControllerSystem : GameSystemExtensionBase {
    private EntityQuery signatureBuildingGroup;
    protected override void OnCreate() {
        base.OnCreate();
        signatureBuildingGroup = GetEntityQuery(new ComponentType[]{
            ComponentType.ReadWrite<SignatureBuildingData>()
        });
        RequireForUpdate(signatureBuildingGroup);
    }

    protected override void OnGameLoaded(Context serializationContext) {
        base.OnGameLoaded(serializationContext);
        var t = signatureBuildingGroup.ToEntityArray(Allocator.TempJob);
        for (int i = 0; i < t.Length; i++) {
            var ts = EntityManager.GetComponentData<PlaceableObjectData>(t[i]);
            ts.m_Flags ^= PlacementFlags.Unique;
            EntityManager.SetComponentData<PlaceableObjectData>(t[i], ts);
            Logger.Info(ts.m_Flags.ToString());
        }
    }

}
