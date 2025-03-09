import { Ionicons } from '@expo/vector-icons';
import { Text, TouchableOpacity, View } from 'react-native';

type Props = {
	title: string
	description: string
	icon: string
	onPress?: () => void
}

const ScreenOptionButton = (prop: Props) => {
	return (
		<TouchableOpacity onPress={prop.onPress}>
			<View className="flex flex-row items-center gap-4 h-[50px]">
				<Ionicons className="text-slate-900 dark:!text-slate-50" name={prop.icon as 'search'} size={22} />
				<View className="flex-1">
					<Text className="font-bold text-slate-900 dark:text-slate-50">{prop.title}</Text>
					<Text className="text-sm text-slate-500">{prop.description}</Text>
				</View>
				<Ionicons className="text-slate-900 dark:!text-slate-50" name="chevron-forward" />
			</View>
		</TouchableOpacity>
	);
};

export default ScreenOptionButton;