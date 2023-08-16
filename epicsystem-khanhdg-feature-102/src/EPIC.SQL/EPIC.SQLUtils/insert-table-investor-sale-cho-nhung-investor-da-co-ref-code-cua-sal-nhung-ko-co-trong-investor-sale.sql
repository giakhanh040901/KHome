DECLARE 
	v_REF_CODE_SELF varchar2(50);
	v_SALE_ID NUMBER;
BEGIN
	for o in (
		SELECT i.INVESTOR_ID, i.PHONE, i.STATUS, i.REFERRAL_CODE, COUNT(s.ID) AS NUM_SALE, i.CREATED_DATE  FROM EP_INVESTOR_SALE s RIGHT JOIN EP_INVESTOR i ON s.INVESTOR_ID = i.INVESTOR_ID 
		WHERE i.CREATED_DATE >= TO_DATE('2022-01-01', 'yyyy-mm-dd') AND i.REFERRAL_CODE IS NOT NULL AND i.DELETED = 'N' AND i.STATUS = 'A'
			--AND i.PHONE IN ('0987693555', '0987693222', '0987987777')
		GROUP BY i.INVESTOR_ID, i.PHONE, i.CREATED_DATE, i.REFERRAL_CODE, i.DELETED, i.STATUS
		HAVING COUNT(s.ID) = 0
		ORDER BY i.CREATED_DATE DESC
	)
  LOOP
	BEGIN 
		SELECT s.SALE_ID INTO v_SALE_ID
		FROM EP_CORE_SALE s
	    --Tìm mã giới thiệu là khách hàng cá nhân
	    LEFT JOIN EP_INVESTOR i ON s.INVESTOR_ID = i.INVESTOR_ID AND i.DELETED = 'N'
	    -- Tìm mã giới thiệu là khách hàng doanh nghiệp
	    LEFT JOIN EP_CORE_BUSINESS_CUSTOMERS bs ON s.BUSINESS_CUSTOMER_ID = bs.BUSINESS_CUSTOMER_ID AND bs.DELETED = 'N'
    	WHERE o.REFERRAL_CODE IN (i.REFERRAL_CODE_SELF, bs.REFERRAL_CODE_SELF ) AND ROWNUM = 1 AND  s.DELETED = 'N';
   
   		
   	
	   	INSERT INTO EP_INVESTOR_SALE
		(
			ID, INVESTOR_ID, SALE_ID, REFERRAL_CODE, IS_DEFAULT 
		)
		VALUES
		(
			SEQ_INVESTOR_SALE.NEXTVAL, o.investor_id, v_SALE_ID, o.REFERRAL_CODE, 'Y'
		);
	
		COMMIT;
		dbms_output.put_line(o.REFERRAL_CODE || ' - ' || v_SALE_ID);
	EXCEPTION 
		WHEN no_data_found THEN
			dbms_output.put_line(o.REFERRAL_CODE);
	END;
  end loop;
END;