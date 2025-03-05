 # -- lo primero es settear los valores que nos preocupan
        grafo = 'digraph T{ \nnode[shape=box fontname="Arial" fillcolor="white" style=filled ]'
        grafo += '\nroot[label = \"capa: ' + str(self.capa) + '\", group=1]\n'
        grafo += '''label = "{}" \nfontname="Arial Black" \nfontsize="15pt" \n
                    \n'''.format('MATRIZ DISPERSA')

        # --- lo siguiente es escribir los nodos encabezados, empezamos con las filas, los nodos tendran el foramto Fn
        x_fila = self.filas.primero
        while x_fila != None:
            grafo += 'F{}[label="F{}",fillcolor="plum",group=1];\n'.format(
                x_fila.id, x_fila.id)
            x_fila = x_fila.siguiente

        # --- apuntamos los nodos F entre ellos
        x_fila = self.filas.primero
        while x_fila != None:
            if x_fila.siguiente != None:
                grafo += 'F{}->F{};\n'.format(x_fila.id, x_fila.siguiente.id)
                grafo += 'F{}->F{};\n'.format(x_fila.siguiente.id, x_fila.id)
            x_fila = x_fila.siguiente

        # --- Luego de los nodos encabezados fila, seguimos con las columnas, los nodos tendran el foramto Cn
        y_columna = self.columnas.primero
        while y_columna != None:
            group = int(y_columna.id)+1
            grafo += 'C{}[label="C{}",fillcolor="powderblue",group={}];\n'.format(
                y_columna.id, y_columna.id, str(group))
            y_columna = y_columna.siguiente

        # --- apuntamos los nodos C entre ellos
        cont = 0
        y_columna = self.columnas.primero
        while y_columna is not None:
            if y_columna.siguiente is not None:
                grafo += 'C{}->C{}\n'.format(y_columna.id,
                                             y_columna.siguiente.id)
                grafo += 'C{}->C{}\n'.format(y_columna.siguiente.id,
                                             y_columna.id)
            cont += 1
            y_columna = y_columna.siguiente

        # --- luego que hemos escrito todos los nodos encabezado, apuntamos el nodo root hacua ellos
        y_columna = self.columnas.primero
        x_fila = self.filas.primero
        grafo += 'root->F{};\n root->C{};\n'.format(x_fila.id, y_columna.id)
        grafo += '{rank=same;root;'
        cont = 0
        y_columna = self.columnas.primero
        while y_columna != None:
            grafo += 'C{};'.format(y_columna.id)
            cont += 1
            y_columna = y_columna.siguiente
        grafo += '}\n'
        aux = self.filas.primero
        aux2 = aux.acceso
        cont = 0
        while aux != None:
            cont += 1
            while aux2 != None:
                # if aux2.caracter == '-':
                #    grafo += 'N{}_{}[label=" ",group="{}"];\n'.format(aux2.x, aux2.y, int(aux2.y)+1)
                # el
                if aux2.caracter == '*':
                    grafo += 'N{}_{}[label="{}",group="{}", fillcolor="black"];\n'.format(
                        aux2.x, aux2.y, aux2.caracter, int(aux2.y)+1)
                elif aux2.caracter == 'E':
                    grafo += 'N{}_{}[label="{}",group="{}", fillcolor="green"];\n'.format(
                        aux2.x, aux2.y, aux2.caracter, int(aux2.y)+1)
                elif aux2.caracter == ' ':
                    grafo += 'N{}_{}[label="{}",group="{}", fillcolor="white"];\n'.format(
                        aux2.x, aux2.y, aux2.caracter, int(aux2.y)+1)
                elif aux2.caracter.isdigit():
                    grafo += 'N{}_{}[label="{}",group="{}", fillcolor="red"];\n'.format(
                        aux2.x, aux2.y, 'U', int(aux2.y)+1)
                elif aux2.caracter == 'C':
                    grafo += 'N{}_{}[label="{}",group="{}", fillcolor="blue"];\n'.format(
                        aux2.x, aux2.y, aux2.caracter, int(aux2.y)+1)
                elif aux2.caracter == 'R':
                    grafo += 'N{}_{}[label="{}",group="{}", fillcolor="gray"];\n'.format(
                        aux2.x, aux2.y, aux2.caracter, int(aux2.y)+1)
                elif aux2.caracter == 'w':
                    grafo += 'N{}_{}[label="{}",group="{}", fillcolor="yellow"];\n'.format(
                        aux2.x, aux2.y, aux2.caracter, int(aux2.y)+1)
                aux2 = aux2.derecha
            aux = aux.siguiente
            if aux != None:
                aux2 = aux.acceso
        aux = self.filas.primero
        aux2 = aux.acceso
        cont = 0
        while aux is not None:
            rank = '{'+f'rank = same;F{aux.id};'
            cont = 0
            while aux2 != None:
                if cont == 0:
                    grafo += 'F{}->N{}_{};\n'.format(aux.id, aux2.x, aux2.y)
                    grafo += 'N{}_{}->F{};\n'.format(aux2.x, aux2.y, aux.id)
                    cont += 1
                if aux2.derecha != None:
                    grafo += 'N{}_{}->N{}_{};\n'.format(
                        aux2.x, aux2.y, aux2.derecha.x, aux2.derecha.y)
                    grafo += 'N{}_{}->N{}_{};\n'.format(
                        aux2.derecha.x, aux2.derecha.y, aux2.x, aux2.y)

                rank += 'N{}_{};'.format(aux2.x, aux2.y)
                aux2 = aux2.derecha
            aux = aux.siguiente
            if aux != None:
                aux2 = aux.acceso
            grafo += rank+'}\n'
        aux = self.columnas.primero
        aux2 = aux.acceso
        cont = 0
        while aux != None:
            cont = 0
            grafo += ''
            while aux2 != None:
                if cont == 0:
                    grafo += 'C{}->N{}_{};\n'.format(aux.id, aux2.x, aux2.y)
                    grafo += 'N{}_{}->C{};\n'.format(aux2.x, aux2.y, aux.id)
                    cont += 1
                if aux2.abajo != None:
                    grafo += 'N{}_{}->N{}_{};\n'.format(
                        aux2.abajo.x, aux2.abajo.y, aux2.x, aux2.y)
                    grafo += 'N{}_{}->N{}_{};\n'.format(
                        aux2.x, aux2.y, aux2.abajo.x, aux2.abajo.y)
                aux2 = aux2.abajo
            aux = aux.siguiente
            if aux != None:
                aux2 = aux.acceso
        grafo += '}'
