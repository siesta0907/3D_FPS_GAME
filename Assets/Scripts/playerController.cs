 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //�ӵ� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    //���� ����
    private bool iswalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    private Vector3 LastPos;

    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;
    //�� ���� ����
    private CapsuleCollider capsuleCollider;

    //ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera thecamera;
    [SerializeField]
    private Rigidbody myRigid;
    private GunController theGunController;
    private CrossHair theCrossHiar;

    //ó�� ������ �� �θ��� �Լ�
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        theGunController = FindObjectOfType<GunController>();
        theCrossHiar = FindObjectOfType<CrossHair>();
        
        
        applySpeed = walkSpeed;
        GameObject thecamera = GameObject.Find("Main Camera");
        myRigid = GetComponent<Rigidbody>();
        originPosY = thecamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    //�����Ӹ��� �ҷ����� �Լ�
    void Update()
    {
        CheckIsGround();
        TryJump();
        TryRun(); //�ݵ�� ���� ���� �־�� ��
        TryCrouch();
        Move();
       
        MoveCheck();

        CameraRotation();
        CharacterRotation();
        
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrossHiar.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
        
    }

    //�ڷ�ƾ ���� ó��!!
    IEnumerator CrouchCoroutine()
    {
        float _posy = thecamera.transform.localPosition.y;
        int count = 0;
        while(_posy != applyCrouchPosY)
        {
            count++;
            _posy = Mathf.Lerp(_posy, applyCrouchPosY, 0.3f);
            thecamera.transform.localPosition = new Vector3(0, _posy, 0);
            if (count > 15) break;
            yield return null;
        }
        thecamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }
    private void CheckIsGround()
    {
        //����ĳ��Ʈ!!
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y +  0.1f);

    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jump();
        }
    }
    private void jump()
    {
        if (isCrouch) Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
       if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
       if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    private void Running()
    {
        if (isCrouch) Crouch();

        
        isRun = true;
        theCrossHiar.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        theCrossHiar.RunningAnimation(isRun); //?
        applySpeed = walkSpeed;
    }
    private void CharacterRotation()
    {
        //�¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
    private void CameraRotation()
    {
        //���� ī�޶� ȸ�� 
        float _xRotiation = Input.GetAxisRaw("Mouse Y");

        float _cameraRoatationX = _xRotiation * lookSensitivity;

        currentCameraRotationX -= _cameraRoatationX;
        //currentCameraRotationX1 += _cameraRoatationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        thecamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
       // thecamera2.transform.localEulerAngles = new Vector3(currentCameraRotationX1, 0f, 0f);
    
}
    private void Move()
    {
        float _movedirX = Input.GetAxisRaw("Horizontal");
        float _movedirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _movedirX;
        Vector3 _moveVertical = transform.forward * _movedirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);


    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch)
        {
            if (Vector3.Distance(LastPos, transform.position) >= 0.1f) 
                iswalk = true;
            else 
                 iswalk = false;

            theCrossHiar.WalkingAnimation(iswalk);
            LastPos = transform.position;
        }
    }
}
