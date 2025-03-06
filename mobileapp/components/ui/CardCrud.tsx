import { Text, TouchableOpacity, View, ViewProps } from 'react-native';
import Ionicons from '@expo/vector-icons/Ionicons';

interface Props extends ViewProps {
	item: any
	onDelete: (item: any) => void,
	onEdit: (item: any) => void,
	disabledEdit?: boolean,
	disabledDelete?: boolean,
}

const CardCrud = ({ item, onDelete, onEdit, children, disabledEdit = false, disabledDelete = false }: Props) => {
	return (
		<View className="w-full flex p-0 bg-slate-100 border border-slate-300 dark:border-slate-600 dark:!bg-slate-800 rounded-lg">
			<View className="flex flex-1 gap-4 p-4">
				{children}
			</View>

			<View className="grid grid-cols-2 border-t border-slate-300 dark:border-slate-600">
				<TouchableOpacity
					disabled={disabledEdit}
					className="flex flex-row w-full gap-2 h-[40px] justify-center items-center border-r border-slate-300 dark:border-slate-600"
					onPress={() => onEdit(item)}
				>
					<Ionicons name="pencil-outline" className="!text-slate-600" size={14} />
					<Text className="font-semibold text-slate-600 dark:text-slate-200">Editar</Text>
				</TouchableOpacity>

				<TouchableOpacity
					disabled={disabledDelete}
					className="flex flex-row w-full gap-2 h-[40px] justify-center items-center"
					onPress={() => onDelete(item)}
				>
					<Ionicons name="trash-outline" className="text-red-500" size={14} />
					<Text className="font-semibold text-red-500">Remover</Text>
				</TouchableOpacity>
			</View>
		</View>
	);
};

export default CardCrud;