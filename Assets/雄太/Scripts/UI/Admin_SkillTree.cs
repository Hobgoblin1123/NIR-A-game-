using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Admin_SkillTree : MonoBehaviour
{
    public SkillsTreeUnite[] skillsTreeUnites;
    [System.Serializable]
    public struct SkillsTreeUnite
    {
        public Skills[] skills;
        public bool isEnd;
        public int nextBranchNumber;
    }

    [System.Serializable]
    public struct Skills
    {
        public SkillTreeButton button;
        public SkillType skillType;
        public float addStatus;
        public bool skillOn;
        public int needPoint;
    }

    [SerializeField]
    private Admin_UI admin_UI;
    private Admin admin;
    private Admin_Date admin_Date;

    public enum SkillType
    {
        RaiseAttack,
        RaiseHP,
        RaiseDefence,
        RaiseDamage,
        SPSkill1,
        None
    }

    [SerializeField]
    public static float AllRaiseAttack;
    [SerializeField]
    public static float AllRaiseHP;
    [SerializeField]
    public static float AllRaiseDefence;
    [SerializeField]
    public static float AllRaiseDamage;

    
    // Start is called before the first frame update
    void Start()
    {
        admin_UI = GetComponentInParent<Admin_UI>();
        admin = admin_UI.admin;
        admin_Date = admin_UI.admin_Date;

        for (int i = 0; i < skillsTreeUnites.Length; i++)
        {
            var n = PlayerPrefs.GetInt(i.ToString());
            for (int t = 0; t < skillsTreeUnites[i].skills.Length; t++)
            {
                if(t <= n)
                {
                    var g = skillsTreeUnites[i].skills[t];
                    g.skillOn = true;
                    g.button.StartSet();

                    if(g.skillType == SkillType.RaiseAttack)
                    {
                        AllRaiseAttack += g.addStatus;
                        Debug.Log(AllRaiseAttack);
                    }
                    else if(g.skillType == SkillType.RaiseHP)
                    {
                        AllRaiseHP += g.addStatus;
                        
                    }
                    else if(g.skillType == SkillType.RaiseDefence)
                    {
                        AllRaiseDefence += g.addStatus;
                    }
                    else if(g.skillType == SkillType.RaiseDamage)
                    {
                        AllRaiseDamage += g.addStatus;
                    }
                    else if(g.skillType == SkillType.SPSkill1)
                    {
                        Debug.Log("なんか特別なやつもらえるらしいけどなにも実装してないでwwwww");
                    }
                    admin.RaiseHPBySkillTree();
                }
            }
        }
    }

    // Update is called once per frame
    
    public void SkillGet(int treeUniteType , int skillUnmder)
    {
        var g = skillsTreeUnites[treeUniteType].skills[skillUnmder];
        if(skillsTreeUnites[treeUniteType].skills[skillUnmder-1].skillOn == true)
        {
            if(admin.skillPoints > g.needPoint)
            {
                g.skillOn = true;
                skillsTreeUnites[treeUniteType].skills[skillUnmder].skillOn= true;
                g.button.SkillOn();
                Debug.Log("取得");


                // 各ステータスの上昇値を設定
                if(g.skillType == SkillType.RaiseAttack)
                {
                    AllRaiseAttack += g.addStatus;
                    Debug.Log(AllRaiseAttack);
                }
                else if(g.skillType == SkillType.RaiseHP)
                {
                    AllRaiseHP += g.addStatus;
                    admin.RaiseHPBySkillTree();
                }
                else if(g.skillType == SkillType.RaiseDefence)
                {
                    AllRaiseDefence += g.addStatus;
                }
                else if(g.skillType == SkillType.RaiseDamage)
                {
                    AllRaiseDamage += g.addStatus;
                }
                else if(g.skillType == SkillType.SPSkill1)
                {
                    Debug.Log("なんか特別なやつもらえるらしいけどなにも実装してないでwwwww");
                }



                if(skillUnmder == skillsTreeUnites[treeUniteType].skills.Length-1)
                {
                    skillsTreeUnites[treeUniteType].isEnd = true;
                    if(JudgeNextLevel(skillsTreeUnites[treeUniteType].nextBranchNumber) == true)
                    {
                        skillsTreeUnites[skillsTreeUnites[treeUniteType].nextBranchNumber].skills[0].skillOn = true;
                        Debug.Log("こい....高見えぇぇぇぇぇ");
                    }
                    else
                    {
                        Debug.Log("次の開放はまだ無理だね");
                    }
                }
            }
            else
            {
                Debug.Log("ポイントが足りませんよ");
            }
            
        }
        else
        {
            Debug.Log("まだ取れませんよ");
        }

        admin_UI.ChangeSkillTree = true;
    }
    private bool JudgeNextLevel(int nextTreeNumber)
    {
        if(nextTreeNumber < 0)return false;

        List<int> array = new List<int>();
        for (int i = 0; i < skillsTreeUnites.Length; i++)
        {
            if (skillsTreeUnites[i].nextBranchNumber == nextTreeNumber)
            {
                array.Add(i);
            }
        }
        foreach (var item in array)
        {
            if(skillsTreeUnites[item].isEnd == false)return false;
        }
        return true;
    }
}
