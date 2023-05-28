using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //���� ������ �ִ� �Ÿ�
    private bool pickupActivated = false; //���� ������ �� true
    private RaycastHit hitInfo; //�浹ü ���� ����

    //������ ���̾�� �����ϵ��� ���̾� ����ũ�� ����
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }
    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUP();
        }
    }
    void CanPickUP()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log("�� ȹ��");
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
    void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else ItemInfoDisappear();
    }
    void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.ItemName + " (E)";

    }
    void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
