import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Ionicons from '@expo/vector-icons/Ionicons';
import { View } from 'react-native';

interface AuthHeaderProps {
	icon: string,
	title: string,
	description: string,
}

const AuthHeader = ({ title, description, icon }: AuthHeaderProps) => {
	return (
		<ThemedView>
			<View className="w-full flex justify-center items-center h-[200px]">
				<View className="w-[70px] h-[70px] bg-primary-500 rounded-full flex justify-center items-center">
					<Ionicons name={icon as 'search'} size={32} className="text-white" />
				</View>
			</View>

			<View className="flex flex-row items-center w-full gap-2">
				<ThemedText className="text-2xl">{title}</ThemedText>
			</View>

			<View className="w-full">
				<ThemedText>{description}</ThemedText>
			</View>
		</ThemedView>
	);
};

export default AuthHeader;