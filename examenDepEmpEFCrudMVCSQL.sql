create database examenDepEmpEFCrudMVC
use examenDepEmpEFCrudMVC

create table Departamento 
(
	idDepartamento int identity(1,1) not null,
	nombreDepartamento varchar(50) not null,
	constraint PK_Departamento primary key (idDepartamento)
)
create table Empleado
(
	idEmpleado int identity(1,1) not null,
	nombreEmpleado varchar(100) not null,
	idDepartamento int not null,
	constraint PK_Empleado primary key (idEmpleado),
	constraint FK_EmpleadoDepartamento foreign key (idDepartamento)
									   references Departamento(idDepartamento)
)
insert into Departamento(nombreDepartamento)values('Ventas'),('Compras'),('TI'),('RH'),('DISEÑO')
insert into Empleado(nombreEmpleado,idDepartamento)
values('Alan',1),('Adrian',2),('Tomas',3),('Carlos',4),('Lilia',5)

--=================== CREAR PROCEDIMIENTOS ALMACENADOS PARA EMPLEADOS =============================

create procedure sp_ListarEmpleadoPorDep
(
	@idDep int
)
as
begin
	select e.idEmpleado, e.nombreEmpleado, d.idDepartamento, d.nombreDepartamento 
	from Empleado e inner join Departamento d 
	on e.idDepartamento = d.idDepartamento
	where d.idDepartamento = @idDep
end
