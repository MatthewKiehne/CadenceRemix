using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

    private bool portExists = false;

    private SerialPort serial;
    private string buffer = "";
    private Rigidbody2D body;

    private Vector2 velocity = Vector2.zero;
    private float movementSpeed = 4f;

    private bool isDashing = false;
    private Vector2 startDashPos;
    private readonly float dashDistanceMax = 1.5f;
    private float dashDistancePassed = 0f;
    private readonly float dashSpeed = 20f;
    private Vector2 dashDirection = Vector2.zero;

    private void Start() {

        this.body = this.GetComponent<Rigidbody2D>();
        this.velocity = Vector2.zero;
        buffer = "";

        string portName = "COM5";
        string[] ports = SerialPort.GetPortNames();
        int portCounter = 0;

        while (!this.portExists && portCounter < ports.Length) {

            if (ports[portCounter].Equals(portName)) {
                this.portExists = true;
            }
            portCounter++;
        }


        if (this.portExists) {
            serial = new SerialPort(portName, 115200);
            serial.Open();
            serial.ReadTimeout = 1;
        }
    }

    private void Update() {

        if (this.portExists) {
            try {
                this.buffer = serial.ReadLine();

                int rightBracket = buffer.LastIndexOf(']');

                if (rightBracket != -1) {

                    int leftBracket = buffer.LastIndexOf('[', rightBracket - 1);

                    if (leftBracket != -1) {

                        //Debug.Log(buffer);

                        //parse string
                        string payload = buffer.Substring(leftBracket + 1, (rightBracket - leftBracket) - 1);
                        string[] tokens = payload.Split(' ');
                        int x, y, d;
                        int.TryParse(tokens[0], out x);
                        int.TryParse(tokens[1], out y);
                        int.TryParse(tokens[2], out d);
                        Vector2 input = new Vector2(x, y);
                        
                        if(d == 0) {
                            this.dashSetUp();
                        }

                        //normal math
                        Vector2 scaledDown = input / 50;
                        Vector2 removeDec = new Vector2((int)scaledDown.x, (int)scaledDown.y);
                        Vector2 fraction = removeDec / new Vector2(10, 10);
                        fraction.x = Mathf.Abs(fraction.x);
                        fraction.y = Mathf.Abs(fraction.y);
                        Vector2 normal = removeDec.normalized;
                        Vector2 calVelocity = normal * fraction;

                        Debug.Log("input: " + input);
                        this.velocity = ((calVelocity * .7f * this.movementSpeed) + (this.velocity * .3f));

                        this.buffer = this.buffer.Substring(rightBracket + 1);

                    } else {
                        this.buffer = "";
                    }
                }
            } catch (System.TimeoutException) {
            }
        } else {

            //keyboard inppus
            Vector2 keys = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            this.velocity = keys * this.movementSpeed;

            if (Input.GetKeyDown(KeyCode.Space)) {
                this.dashSetUp();
            }
        }

        if (this.isDashing) {

            this.body.velocity = this.dashDirection * this.dashSpeed;

            if(Vector2.Distance(this.startDashPos, this.transform.position) > this.dashDistanceMax) {

                this.isDashing = false;
                float distance = Vector2.Distance(this.transform.position, this.startDashPos);
            }

        } else {
            this.body.velocity = this.velocity;
        }

        //rotates the player
        if (Mathf.Abs(this.body.velocity.x) > .05f || Mathf.Abs(this.body.velocity.y) > .05f) {
            Vector2 rotation = this.body.velocity;
            float angle = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) + 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        //keeps within screen
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void OnTriggerEnter2D(Collider2D coll) {

        GameObject.Destroy(coll.gameObject);
        Debug.Log(coll);
    }

    private void dashSetUp() {
        if (this.velocity != Vector2.zero) {
            this.isDashing = true;
            this.dashDirection = this.velocity.normalized;
            this.dashDistancePassed = 0f;
            this.startDashPos = this.transform.position;
        }
    }

    public bool IsDashing {
        get {
            return this.isDashing;
        }
    }
}
