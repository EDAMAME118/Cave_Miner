using UnityEngine;

public static class Constants
{
    //-------------------
    //アップグレード定数
    //-------------------

    //採掘速度
    public const float MINING_SPEED_COST_INCREASE_MAX_LEVEL = 70; //必要コストが上がらないようになるまでのレベル
    public const float MINING_SPEED_UPGRADE_VALUE = 0.08f;        //採掘速度の上昇値
    public const float MINING_SPEED_COST_INCREASE_RATE = 0.1f;    //必要コストにかける値

    //採掘範囲
    public const int   MAX_MINING_RANGE = 15;                  //採掘範囲の最大レベル
    public const float MINING_RANGE_UPGRADE_VALUE = 0.25f;     //採掘範囲の上り幅
    public const float MINING_RANGE_OFFSET_VALUE  = 0.125f;    //採掘範囲上昇時のオフセット移動値
    public const float MINING_RANGE_COST_INCREASE_RATE = 0.5f; //必要コストにかける値

    //移動速度
    public const int   MAX_MOVE_SPEED = 60;                 //移動速度の最大レベル
    public const float MOVE_SPEED_UPGRADE_VALUE = 0.1f;     //移動速度の上昇値
    public const float MOVE_SPEED_COST_INCREASE_RATE = 0.2f;//必要コストにかける値

    //強化時各種強化レベルに加算される値
    public const int   LEVEL_ADD = 1;

    // 強化項目の右端のインデックス（項目数が3つの場合、0, 1, 2 なので 2）
    public const int UPGRADE_MAX_INDEX = 2;
    // 左端のインデックス
    public const int UPGRADE_MIN_INDEX = 0;
}
