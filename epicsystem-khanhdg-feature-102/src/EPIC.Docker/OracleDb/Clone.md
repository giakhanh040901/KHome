1. dump file
    - cấu hình đường dẫn lưu file dpdmp
        ```console
        su oracle
        sqlplus / as sysdba
        SQL> select * from dba_directories;
        ```
    - nếu chưa có DATA_PUMP_DIR
        ```console
        SQL> create directory DATA_PUMP_DIR as '/opt/oracle/admin/ORCLCDB/dpdump/';
        ```
    - nếu chưa grant quyền truy cập vào directory DATA_PUMP_DIR cho các schema khác
        ```console
        SQL> GRANT READ, WRITE ON DIRECTORY DATA_PUMP_DIR TO EPIC;
        # GRANT READ, WRITE ON DIRECTORY DATA_PUMP_DIR TO EPIC_GARNER;
        # GRANT READ, WRITE ON DIRECTORY DATA_PUMP_DIR TO EPIC_REAL_ESTATE;
        # GRANT READ, WRITE ON DIRECTORY DATA_PUMP_DIR TO EPIC_LOYALTY;
        # GRANT READ, WRITE ON DIRECTORY DATA_PUMP_DIR TO EPIC_COMPANY_SHARES;

        # REVOKE READ, WRITE ON DIRECTORY DATA_PUMP_DIR FROM EPIC_GARNER;
        # REVOKE READ, WRITE ON DIRECTORY DATA_PUMP_DIR FROM EPIC_REAL_ESTATE;
        # REVOKE READ, WRITE ON DIRECTORY DATA_PUMP_DIR FROM EPIC_LOYALTY;
        # REVOKE READ, WRITE ON DIRECTORY DATA_PUMP_DIR FROM EPIC_COMPANY_SHARES;
        ```
    - xem quyền đã gán
        ```SQL
        SELECT grantee FROM all_tab_privs WHERE table_name = 'DATA_PUMP_DIR';
        ```
    - thoát sqlplus và chạy lệnh dump với tài khoản sys
        ```sh
        expdp \"sys/<pw> as sysdba\" schemas=EPIC dumpfile=EPIC.dpdmp REUSE_DUMPFILES=YES
        ```
        - *REUSE_DUMPFILES*: ghi đè lên file dpdmp đã tồn tại
    - với có thể dùng tài khoản có role dba
        ```sh
        expdp EPIC/<pw> schemas=EPIC_GARNER dumpfile=EPIC_GARNER.dpdmp REUSE_DUMPFILES=YES;
        ```
    - trường hợp user schema này password bị expired thì vào lại sqlplus / as sysdba và chạy lệnh
        ```sh
        ALTER USER <tên user schema> IDENTIFIED BY <new_password>;
        # ALTER USER EPIC IDENTIFIED BY <new_password>;
        # ALTER USER EPIC_GARNER IDENTIFIED BY <new_password>;
        # ALTER USER EPIC_COMPANY_SHARES IDENTIFIED BY <new_password>;
        # ALTER USER EPIC_REAL_ESTATE IDENTIFIED BY <new_password>;
        # ALTER USER EPIC_LOYALTY IDENTIFIED BY <new_password>;
        ```
2. copy file từ server khác thông qua scp
    ```sh
    scp root@172.16.0.25:/opt/oracle/admin/ORCLCDB/dpdump/EPIC.dpdmp /opt/oracle/admin/EPICDEV/dpdump
    scp root@172.16.0.25:/opt/oracle/admin/ORCLCDB/dpdump/EPIC_GARNER.dpdmp /opt/oracle/admin/EPICDEV/dpdump
    scp root@172.16.0.25:/opt/oracle/admin/ORCLCDB/dpdump/EPIC_COMPANY_SHARES.dpdmp /opt/oracle/admin/EPICDEV/dpdump
    scp root@172.16.0.25:/opt/oracle/admin/ORCLCDB/dpdump/EPIC_REAL_ESTATE.dpdmp /opt/oracle/admin/EPICDEV/dpdump
    scp root@172.16.0.25:/opt/oracle/admin/ORCLCDB/dpdump/EPIC_LOYALTY.dpdmp /opt/oracle/admin/EPICDEV/dpdump
    # scp root@172.16.0.25:/opt/oracle/admin/ORCLCDB/dpdump/EPIC.dpdmp /opt/oracle/admin/EPICDEV/dpdump
    ```
3. import file dump
    - lỗi bash: impdp: command not found
        - sửa file **~/.bashrc**
            ```sh
            export ORACLE_HOME=/opt/oracle/product/19c/dbhome_1
            export PATH=$ORACLE_HOME/bin:$PATH
            ```
        - chạy ```source ~/.bashrc```
    - lỗi TNS:net service name is incorrectly specified
        - sửa file **~/.bashrc**
            ```sh
            export ORACLE_SID=EPICDEV
            ```
        - chạy ```source ~/.bashrc```

    - xoá các object type không phải table trước khi import (sequence và package)
        ```SQL
        DECLARE
	        vCMD varchar(512);
        BEGIN
            FOR obj IN (SELECT * FROM ALL_OBJECTS WHERE OWNER = 'EPIC' 
                AND (OBJECT_TYPE = 'SEQUENCE' OR OBJECT_TYPE = 'PACKAGE')) LOOP
                    vCMD := 'DROP ' || obj.OBJECT_TYPE || ' ' || obj.OWNER || '.' || obj.OBJECT_NAME;
                    DBMS_OUTPUT.PUT_LINE(vCMD);
                    EXECUTE IMMEDIATE vCMD;
            END LOOP;
        END;
        ```
    - import dùng tài khoản sys
        ```sh
        impdp \"sys/<pw> as sysdba\" directory=DATA_PUMP_DIR dumpfile=EPIC_GARNER.dpdmp schemas=EPIC_GARNER CONTENT=ALL TABLE_EXISTS_ACTION=REPLACE REMAP_SCHEMA=EPIC_GARNER:EPIC_GARNER
        ```
4. trường hợp chưa cài password cho oracle linux
    ```
    sudo passwd oracle #sau đó nhập password cho user này
    ```
5. copy file
    ```sh
    scp -r root@172.16.0.100:'/app/App_Data/*' /app/App_Data/
    ```
    ```sh
    docker run --name oracle-db -p 1521:1521 -p 5500:5500 -e ORACLE_PWD=123456 -v oradata:/opt/oracle/oradata container-registry.oracle.com/database/express:21.3.0-xe
    ```
