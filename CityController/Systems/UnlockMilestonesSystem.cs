using CityController.Settings;
using Colossal.Serialization.Entities;
using CS2Shared.Common;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CityController.Systems;

public partial class UnlockMilestonesSystem : GameSystemBaseExtension {
    private EntityArchetype unlockEventArchetype;
    private EntityQuery milestoneLevelGroup;
    private EntityQuery milestoneGroup;
    private CitySystem citySystem;

    protected override void OnCreate() {
        base.OnCreate();
        citySystem = World.GetOrCreateSystemManaged<CitySystem>();
        unlockEventArchetype = EntityManager.CreateArchetype(new ComponentType[] {
            ComponentType.ReadWrite<Event>(),
            ComponentType.ReadWrite<Unlock>()
        });
        milestoneLevelGroup = GetEntityQuery(new ComponentType[] { ComponentType.ReadWrite<MilestoneLevel>() });
        milestoneGroup = GetEntityQuery(new ComponentType[] { ComponentType.ReadOnly<MilestoneData>() });
        RequireForUpdate(milestoneLevelGroup);
        RequireForUpdate(milestoneGroup);
    }


    protected override void OnGameLoaded(Context serializationContext) {
        base.OnGameLoaded(serializationContext);
        if (InGame)
            UnlockMilestone();
    }

    private void UnlockMilestone() {
        if (!Setting.Instance.CustomMilestone)
            return;
        NativeArray<Entity> nativeArray = milestoneGroup.ToEntityArray(Allocator.TempJob);
        NativeArray<MilestoneData> nativeArray2 = milestoneGroup.ToComponentDataArray<MilestoneData>(Allocator.TempJob);
        MilestoneLevel singleton = milestoneLevelGroup.GetSingleton<MilestoneLevel>();
        PlayerMoney componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
        Creditworthiness componentData2 = EntityManager.GetComponentData<Creditworthiness>(citySystem.City);
        DevTreePoints componentData3 = EntityManager.GetComponentData<DevTreePoints>(citySystem.City);
        XP componentData4 = EntityManager.GetComponentData<XP>(citySystem.City);
        try {
            for (int i = singleton.m_AchievedMilestone; i < Setting.Instance.MilestoneLevel + 1; i++) {
                Entity entity = EntityManager.CreateEntity(unlockEventArchetype);
                EntityManager.SetComponentData(entity, new Unlock(nativeArray[i]));
                singleton.m_AchievedMilestone = math.max(singleton.m_AchievedMilestone, nativeArray2[i].m_Index);
                componentData.Add(nativeArray2[i].m_Reward);
                componentData2.m_Amount += nativeArray2[i].m_LoanLimit;
                componentData3.m_Points += nativeArray2[i].m_DevTreePoints;
                componentData4.m_XP = nativeArray2[i].m_XpRequried;
            }
        }
        finally {
            nativeArray.Dispose();
            nativeArray2.Dispose();
        }
        milestoneLevelGroup.SetSingleton(singleton);
        EntityManager.SetComponentData(citySystem.City, componentData);
        EntityManager.SetComponentData(citySystem.City, componentData2);
        EntityManager.SetComponentData(citySystem.City, componentData3);
        EntityManager.SetComponentData(citySystem.City, componentData4);
        Logger.Info($"Unlock level {Setting.Instance.MilestoneLevel + 1} Milestone");
    }

}
