<?php
	// link de la web donde esta el servidor
	$server = "";

	// nombre del usuario para acceder a la base de datos
	$user = "";

	// contraseña para acceder a la base de datos
	$password = "";

	// nombre de la base de datos a acceder
	$database = "";

	// nombre de la tabla a acceder
	$table = "";

    $conn = mysqli_connect($server, $user, $password , $database);

    // si no se pudo conectar a la base de datos, manda un mensaje de error
    if (!$conn){
        echo "error de conexión";
    }
?>

<!DOCTYPE html>
<html>
    <body>
        <table>
            <tr>
                <td>User</td>
                <td>Skin</td>
                <td>Score</td>
                <br>
            </tr>
            <?php
            // esta parte es escensial
            // si la conexion a la base de datos fue exitosa, buscara todos los datos de la tabla y los ordenara por el Score de mayor a menor
                $sqlTable = "SELECT * FROM $table ORDER BY Score DESC";
                $invokeTable = mysqli_query($conn, $sqlTable);

                while($show = mysqli_fetch_array($invokeTable)){
            ?>
            <tr>
                    <td><?php echo "|" . $show["Usuario"] . "|" ?></td>
                    <td><?php echo "|" . $show["Skin"] . "|" ?></td>
                    <td><?php echo "|" . $show["Score"]    . "|" ?></td>
            </tr>
            <?php
            // la parte superior muestra en el navegador la tabla completa
            // es muy importante que se coloque un caracter entre cada dato de la tabla, puede ser el de su preferencia (en este caso es '|')
                }
            ?>
        </table>
    </body>
</html>