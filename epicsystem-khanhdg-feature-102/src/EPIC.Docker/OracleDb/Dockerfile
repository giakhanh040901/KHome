FROM container-registry.oracle.com/database/express:21.3.0-xe

# # Update sudoers file to allow passwordless su
# RUN echo 'oracle ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers

# # Start the Oracle listener
# RUN echo "startup" | sqlplus -s / as sysdba && \
#     lsnrctl start

# Expose the database port
EXPOSE 1521

#RUN impdp system/chaien directory=/opt/OracleDb dumpfile=EPIC.dpdmp schemas=EPIC full=y