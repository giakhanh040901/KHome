DECLARE 
	v_REF_CODE_SELF varchar2(50);
BEGIN
	for o in (
		SELECT * 
		FROM EP_INVESTOR
		WHERE REFERRAL_CODE_SELF  IS NULL
	)
	LOOP
	v_REF_CODE_SELF := PKG_INVESTOR.FUNC_REFERAL_GENERATE();

	UPDATE EP_INVESTOR 
	SET REFERRAL_CODE_SELF = v_REF_CODE_SELF
	WHERE INVESTOR_ID = o.investor_id;
	COMMIT;
	
	end loop;
END;