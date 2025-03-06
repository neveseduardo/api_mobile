import { ThemedText } from '@/components/ui/ThemedText';
import { Text, View } from 'react-native';

interface Props {
	username?: string,
	email?: string,
}

const UserProfileHeader = ({ username = 'USER NAME', email }: Props) => {
	const initialLetters = username.split(' ').map((l) => l.charAt(0));

	return (
		<View className="w-full min-h-[80px] flex flex-row gap-4 items-center">
			<View className="w-[70px] h-[70px] rounded-full bg-gray-600 flex justify-center items-center">
				<View className="text-2xl font-semibold text-white">
					<Text>{initialLetters}</Text>
				</View>
			</View>
			<View>
				<ThemedText className="font-semibold">Bem vindo!</ThemedText>
				<ThemedText className="uppercase">{username}</ThemedText>
				<ThemedText>{email}</ThemedText>
			</View>
		</View>
	);
};

export default UserProfileHeader;