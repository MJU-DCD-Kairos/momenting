using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class MBTIManager : MonoBehaviour
{
    public Text header;
    public Text desc;
    public string MbtiType;
    public GameObject noTypeHead;
    public GameObject MyType;

    // Start is called before the first frame update
    void Start()
    {
        MbtiType = PlayerPrefs.GetString("MBTIResert");
        mbtiresult();
    }
    public void mbtiresult() { 
   if (MbtiType == "INTP")
        {
            header.text = "���� �𷡾�";
            desc.text = "���ο� ������ �����ϴ� ���̵�� ��ũ����. ���� �ִ� �о߿� ���� ��ȭ�� ������ ȯ��!";
        }
        else if (MbtiType == "INTJ")
{
            header.text = "��Ϲ��� �𷡾�";
            desc.text = "������ ǳ���� ����������. �׻� ö��ö���� ��ȹ�� �ִ�ϴ�.";
        }
else if (MbtiType == "INFP")
{
            header.text = "�޻��¦ �𷡾�";
            desc.text = "�������� ǳ���� ��ȭ�����ڿ���. �β����� ���� Ÿ�� Ÿ���̴� �������� �ٰ����ּ���!";
        }
else if (MbtiType == "INFJ")
{
            header.text = "������ �𷡾�";
            desc.text = "ȭ���� �����ϴ� �̻������ڿ���. ���� �������� ģ������ ��մ� ������ �����ٰԿ�.";
        }
else if (MbtiType == "ISTP")
{
            header.text = "�ư��̹� �𷡾�";
            desc.text = "������ ��������̿���. ���������� ��ġ�� ���� �ֺ� ��Ȳ �ľ��� �� �ؿ�.";
        }
else if (MbtiType == "ISTJ")
{
            header.text = "������ �𷡾�";
            desc.text = "���� ������ ���� ��������. �����ϰ� ���Ǵ����� ������� Ÿ���Դϴ�.";
        }
else if (MbtiType == "ISFP")
{
            header.text = "���� �𷡾�";
            desc.text = "ȣ����� ���� ����������. �ΰ����迡 �����ϰ� �ΰ����迡 ��ó���� �� �־��.";
        }
else if (MbtiType == "ISFJ")
{
            header.text = "�밨���� �𷡾�";
            desc.text = "�ε巴���� ��ȣ�� ���п���. �����ϰ� ������ �ʱ� ������ ���� �α� ���� ������������ Ÿ���̿���.";
        }
else if (MbtiType == "ENTP")
{
            header.text = "�߰ſ� ���� �𷡾�";
            desc.text = "������ ���� �ȿ� �߰ſ� ������ �־��. �����ϴ� ���� ����� �ű⿡ ������!";
        }
else if (MbtiType == "ENTJ")
{
            header.text = "���� ������ �𷡾�";
            desc.text = "���� ������ �������̿���. �׻� �ּ��� ������ �غ�� �ִ�ϴ�.";
        }
else if (MbtiType == "ENFP")
{
            header.text = "����� �𷡾�";
            desc.text = "Ȱ�� ��ġ�� �����̿���. �������̰� �������� ���� ������� �� �������.";
        }
else if (MbtiType == "ENFJ")
{
            header.text = "ī������ �𷡾�";
            desc.text = "�ɼ��ɶ��� �������̿���. � ��Ȳ�̵� ���뼺�ְ� ��Ƶ��ϴ�.";
        }
else if (MbtiType == "ESTP")
{
            header.text = "���谡 �𷡾�";
            desc.text = "������ ���� Ȱ��������. �����ϰ� ���԰��� ���� ���� ��� �� ���� Ÿ���̿���.";
        }
else if (MbtiType == "ESTJ")
{
            header.text = "��Ʈüũ �𷡾�";
            desc.text = "������ �����ڿ���. ��ô ö���ϰ� ������ ���� ���� ������ �ֺ� ������� �Ǹ��� ��ģ��ϴ�.";
        }
else if (MbtiType == "ESFP")
{
            header.text = "�ٶ����� �𷡾�";
            desc.text = "�����ο� ��ȥ�� �����ڿ���.��õ���̰� ������ ���Ӱ� ���� ��� ����ִ� Ÿ���̿���.";
        }
else if (MbtiType == "ESFJ")
{
            header.text = "������� �𷡾�";
            desc.text = "��ȭ�� �߽��ϴ� Ÿ�� �����ڿ���. �ٸ� ������� �� ���� ���� ���� ����� �����ؿ�.";
        }
        else if (MbtiType == "")
        {
            noTypeHead.SetActive(true);
            MyType.SetActive(false);
        }
    }
}
