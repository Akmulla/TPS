using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    private float lookSensitivity = 5;
    float inputX=0,inputY=0;

    private float lookSmoothness = 0.1f;
    public Transform player;
    private const float minLim = 0.0f;
    private const float maxLim = 60.0f;
    private Vector3 _desirePosition;
    //Скорости движения по соответствующим осям
    private float _velX;
    private float _velY;
    private float _velZ;
    //Окончательная позиция камеры
    private Vector3 _position;
    float Distance = 10.0f;


    void Start()
    {
        
    }

    void Update()
    {
        inputX -= Input.GetAxis("Mouse Y") * lookSensitivity;
        inputY += Input.GetAxis("Mouse X") * lookSensitivity;
        inputX = ClampAngle(inputX, minLim, maxLim);
        //print(inputX);
        CalcDesirePosition();
        UpdatePosition();
    }
    private void CalcDesirePosition()
    {
        _desirePosition = CalcPosition(inputX, inputY, Distance);
    }
    private Vector3 CalcPosition(float rotx, float roty, float distance)
    {

        //Точка прямо позади персонажа на расстоянии камеры
        Vector3 direction = new Vector3(0, 0, -distance);

        //Поворот вокруг персонажа на нужный угол
        Quaternion rotation = Quaternion.Euler(rotx, roty, 0);

        //Возвращаем нужную позицию камеры в мировом пространстве
        return player.position + rotation * direction;
    }
    private void UpdatePosition()
    {

        //Сглаживаем движения по соответствующим осям
        //float posX = Mathf.SmoothDamp(_position.x, _desirePosition.x, ref _velX, lookSmoothness);
        //float posY = Mathf.SmoothDamp(_position.y, _desirePosition.y, ref _velY, lookSmoothness);
        //float posZ = Mathf.SmoothDamp(_position.z, _desirePosition.z, ref _velZ, lookSmoothness);
        float posX = _desirePosition.x;
        float posY = _desirePosition.y;
        float posZ = _desirePosition.z;

        //Формируем вектор - окончательное положение камеры
        _position = new Vector3(posX, posY, posZ);

        //Перемещаем камеру в рассчитанное положение
        transform.position = _position;

        //Поворачиваем камеру так, чтобы она глядела на цель
        transform.LookAt(player);
    }
    

    public float ClampAngle(float angle, float min, float max)
    {
        //Делаем
        do
        {
            //Если угол меньше -360 прибавляем к нему 360
            if (angle < -360) angle += 360;

            //Если больше 360 - вычитем
            if (angle > 360) angle -= 360;

            //Пока он не окажется в диапазоне [-360;360]
        } while (angle < -360 || angle > 360);

        //Возвращаем ограничив сверху и снизу
        return Mathf.Clamp(angle, min, max);
    }
}
